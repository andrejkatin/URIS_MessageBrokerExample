using MessageBrokerBus.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CKafka = Confluent.Kafka;

namespace KafkaMessageBus
{
    public class MessageService : MessageBrokerAbstraction
    {
        // Endpoint koji ce se iz koda gadjati. Po defaultu je adresa localhost:9092
        private readonly string KafkaEndpoint = "localhost:9092";

        // Objekti koji ce definisati svu neophodnu konfiguraciju za Producer i Consumer objekte
        private CKafka.ProducerConfig ProducerConfig;
        private CKafka.ConsumerConfig ConsumerConfig;

        // Definicija za kasnije kreiranje konkretnih Producera i Consumera.
        // Kao tipovi podataka navode se tip podatka poruke koja se salje/prima.
        private CKafka.IConsumer<CKafka.Ignore, string> ConsumerBuild = null;
        private CKafka.IProducer<string, string> ProducerBuild = null;

        public string GroupID = "";
        private bool EnableParttitonEof = false;

        private CancellationTokenSource cts;

        // Definisan asinhroni dogadjaj koji kada se desi, sluzice za poziv odgovarajuce metode u okviru klase
        // koja ocekuje trigerovanje dogadjaja
        public override event IMessageService.AsyncEvent MessageArrived;

        public MessageService(string consumerID, string groupID, bool enablePartitionEof) : base(consumerID, groupID, enablePartitionEof)
        {
            GroupID = groupID;
            EnableParttitonEof = enablePartitionEof;
        }

        public MessageService(Guid consumerID, string groupID, bool enablePartitionEof) : base(consumerID.ToString(), groupID, enablePartitionEof)
        {
            GroupID = groupID;
            EnableParttitonEof = enablePartitionEof;
        }

        public MessageService(Guid consumerID, string topic, string group) : base(consumerID, topic, group)
        {
            ConsumerConfig = new CKafka.ConsumerConfig
            {
                GroupId = group,
                BootstrapServers = KafkaEndpoint,
                AutoOffsetReset = CKafka.AutoOffsetReset.Earliest,
            };
        }

        // Metoda za slanje poruke odredjenog sadrzaja na topic koji se prosledjuje kao parametar metode
        public override void Publish(string messageContent, string topic)
        {
            try
            {
                // U okviru konfiguracije Producer-a dodaje se samo Endpoint koji je neophodno gadjati kako bi se
                // poruka prosledila Kafki
                ProducerConfig = new CKafka.ProducerConfig { BootstrapServers = KafkaEndpoint };
                using (ProducerBuild = new CKafka.ProducerBuilder<string, string>(ProducerConfig).Build())
                {
                    // pozivom Produce metode navodi se da Producer posalje poruku sadrzaja koji je prosledjen
                    // kroz parametre metode na definisani topic
                    ProducerBuild.Produce(topic, new CKafka.Message<string, string> { Value = messageContent });
                    ProducerBuild.Flush(TimeSpan.FromSeconds(10));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to produce message!", ex);
            }
        }

        // Metoda koja se koristi prilikom slanja GET zahteva Messaging kontroleru
        // Sluzi da dobavi listu poruka koju je neki servis propustio u toku odredjenog vremena tako sto ce
        // iscitati sve poruke iz Message Queue-a
        public override List<string> Consume(string topic, string group)
        {
            try
            {
                List<string> returnList = new List<string>();

                // U konfiguraciji Consumera se navode svi parametri kako bi se iscitavanje izvrsilo neometano
                // GroupId je particija kojoj se pristupa, BootstrapServers je Endpoint na kojem se kafka nalazi
                // U okviru konfiguracije navodi se jos nacin na koji cete citati poruke (AutoOffsetReset i EnablePartitionEof),
                // Da li zelite da se Topici automatski kreiraju ukoliko ne postoje
                ConsumerConfig = new CKafka.ConsumerConfig
                {
                    GroupId = group,
                    BootstrapServers = KafkaEndpoint,
                    AutoOffsetReset = CKafka.AutoOffsetReset.Earliest,
                    AllowAutoCreateTopics = true,
                    EnablePartitionEof = EnableParttitonEof
                };

                using (var consumerBulid = new CKafka.ConsumerBuilder<CKafka.Ignore, string>(ConsumerConfig).Build())
                {
                    // Consumer se povezuje metodom Subscribe jedan ili vise Topic-a
                    consumerBulid.Subscribe(new List<string>() { topic });

                    // Definisanje Console Key-a za prekid komunikacije
                    CancellationTokenSource cts = new CancellationTokenSource();
                    Console.CancelKeyPress += (_, e) =>
                    {
                        e.Cancel = true;
                        cts.Cancel();
                    };

                    CKafka.ConsumeResult<CKafka.Ignore, string> cr;
                    Console.WriteLine("Consumed messages: ");
                    do
                    {
                        // Citanje poruka pozivom metode Consume koja je definisana za Consumera
                        cr = consumerBulid.Consume();
                        // Citanje poruka vrsi se sekvencijalno, dokle god se ne dodje do kraja Message Queue-a
                        if (!cr.IsPartitionEOF)
                        {
                            Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");

                            returnList.Add(cr.Message.Value);
                        }
                        else
                        {
                            try
                            {
                                // Kada se dodje do kraja Message Queue-a, metodom Commit se pomera Offset na kraj
                                consumerBulid.Commit();
                            }
                            catch (CKafka.KafkaException ex)
                            {
                                Console.WriteLine("MessageService ==> Consumer: " + group + " failed to consume message: " + cr.Message.Value + " from topic: " + topic);
                                Console.WriteLine(ex.Message);
                                continue;
                            }
                        }
                    } while (!cr.IsPartitionEOF);

                }

                return returnList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("MessageService ==> Consumer: " + group + " failed to consume message from topic: " + topic);
                Console.WriteLine(ex.Message);
                return new List<string>();
            }
        }

        // Metoda kojom se Consumer odvezuje sa odredjenog Topica
        public override void Unsubscribe()
        {
            ConsumerBuild.Unsubscribe();
            Console.WriteLine("MessageService ==> Consumer has unsubscribed.");
            cts.Cancel();
        }

        // Metoda Subscribe omogucava povezivanje Consumera sa odredjenim Topicom i particijom u okviru tog Topica
        public override void Subscribe(string topic, string group)
        {
            try
            {
                // Za konfiguraciju je dovoljno navesti particiju (GroupId), kafka Endpoint (BootstrapServers) i
                // Nacin citanja poruka u zavisnosti od Offset-a
                ConsumerConfig = new CKafka.ConsumerConfig
                {
                    GroupId = group,
                    BootstrapServers = KafkaEndpoint,
                    AutoOffsetReset = CKafka.AutoOffsetReset.Earliest,
                };

                // Kreira se Consumer pri cemu se navodi tip podatka poruke koji ce prihvatati
                ConsumerBuild = new CKafka.ConsumerBuilder<CKafka.Ignore, string>(ConsumerConfig).Build();

                // Povezivanje na jedan ili vise Topic-a
                ConsumerBuild.Subscribe(new List<string>() { topic });
                Console.WriteLine("MessageService ==> Consumer: " + group + " subscribed to topic: " + topic);

                cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                // Pozivanje metode za osluskivanje novih dogadjaja u novom thread-u kako bi se omogucilo 
                // opsluzivanje bez blokiranja veceg broja razlicitih Consumer servisa
                RunMethodInSeparateThread(MessageConsume);
            }
            catch (CKafka.KafkaException ex)
            {
                Console.WriteLine("MessageService ==> Consumer: " + group + " failed to consume message from topic: " + topic);
                Console.WriteLine(ex.Message);
                throw new Exception("Failed to consume message", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("MessageService ==> Consumer: " + group + " failed to consume message from topic: " + topic);
                Console.WriteLine(ex.Message);
                throw new Exception("Failed to consume message", ex);
            }
        }

        // Metoda namenjena osluskivanju dogadjaja na odgovarajucem Topic-u
        private void MessageConsume()
        {
            while (true)
            {
                try
                {
                    CKafka.ConsumeResult<CKafka.Ignore, string> cr = ConsumerBuild.Consume(cts.Token);

                    // Thread se blokira dokle god se ne desi neki Event na tom Topic-u. Kada se to desi, poruka
                    // se iscitava i pozivom delegata se trigeruje Event (metodom Invoke)
                    if (!cr.IsPartitionEOF)
                    {
                        Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                        Console.WriteLine("MessageService ==> Consumer: " + ConsumerBuild.Name + " consumed message: " + cr.Message.Value + " from topic: " + ConsumerBuild.Subscription);
                        MessageArrived.Invoke(this, cr.Message.Value);
                    }
                }
                catch (CKafka.ConsumeException ex)
                {
                    Console.WriteLine("MessageService ==> Consumer: failed to consume message.");
                    Console.WriteLine(ex.Message);
                }
                catch (OperationCanceledException ex)
                {
                    if (ConsumerBuild != null)
                    {
                        Console.WriteLine("MessageService ==> Consumer: failed to consume message.");
                        Console.WriteLine(ex.Message);
                        ConsumerBuild.Close();
                    }
                    break;
                }
            }
        }

        // Metoda kreira poseban Thread za odredjenu aktivnost
        private void RunMethodInSeparateThread(Action action)
        {
            try
            {
                var thread = new Thread(new ThreadStart(action));
                thread.Start();
            }
            catch (ThreadStartException ex)
            {
                Console.WriteLine("MessageService ==> Consumer thread can't be started.");
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
