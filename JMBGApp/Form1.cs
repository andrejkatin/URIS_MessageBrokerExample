using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JMBGApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool CheckDate(char[] date)
        { 
            try
            {
                int day1 = int.Parse(date[0].ToString());
                int day2 = int.Parse(date[1].ToString());
                int month1 = int.Parse(date[2].ToString());
                int month2 = int.Parse(date[3].ToString());
                int year1 = int.Parse(date[4].ToString());
                int year2 = int.Parse(date[5].ToString());
                int year3 = int.Parse(date[6].ToString());

                int days = day1 * 10 + day2;
                
                int years = year1 * 100 + year2 * 10 + year3;

                if (years < 100)
                {
                    years = years + 2000;
                }
                else
                {
                    years = years + 1000;
                }

                if ((month1 == 0 && (month2 == 1 || month2 == 3 || month2 == 5 || month2 == 7 || month2 == 8)) || (month1 == 1 && (month2 == 0 || month2 == 2)))
                {
                    if (days <= 31)
                    {
                        return true;
                    }
                    MessageBox.Show("Unet JMBG nije u validnom formatu - mesec ima 31 dana", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (month1 == 0 && month2 == 2)
                {
                    if (years % 4 == 0)
                    {
                        if (days <= 29)
                        {
                            return true;
                        }
                        MessageBox.Show("Unet JMBG nije u validnom formatu - mesec februar ima 29 dana", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (days <= 28)
                        {
                            return true;
                        }
                        MessageBox.Show("Unet JMBG nije u validnom formatu - mesec februar ima 28 dana", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (days <= 30)
                    {
                        return true;
                    }
                    MessageBox.Show("Unet JMBG nije u validnom formatu - mesec ima 30 dana", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch
            {
                MessageBox.Show("Unet JMBG nije u validnom formatu", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private string CheckDays(char[] days)
        {
            try
            {
                int result = 0;
                string res = "";


                int day1 = int.Parse(days[0].ToString());
                int day2 = int.Parse(days[1].ToString());

                if (day1 == 0 && day2 > 0)
                {
                    result = day2;
                    res = result.ToString();
                }
                else if (day1 == 1 || day1 == 2)
                {
                    result = day1*10 + day2;
                    res = result.ToString();
                }
                else if (day1 == 3 && day2 < 2)
                {
                    result = day1 * 10 + day2;
                    res = result.ToString();
                }
                else
                {
                    res = "Unet JMBG nije u validnom formatu";
                }

                return res;
            } 
            catch
            {
                MessageBox.Show("Unet JMBG nije u validnom formatu", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ""; 
                //throw new Exception("Unet JMBG nije u validnom formatu");
            }

        }

        private string CheckMonths(char[] months)
        {
            try
            {
                int result = 0;
                string res = "";


                int month1 = int.Parse(months[0].ToString());
                int month2 = int.Parse(months[1].ToString());

                if (month1 == 0 && month2 > 0)
                {
                    result = month2;
                    res = result.ToString();
                }
                else if (month1 == 1 && month2 < 3)
                {
                    result = month1 * 10 + month2;
                    res = result.ToString();
                }
                else
                {
                    res = "Unet JMBG nije u validnom formatu";
                }

                return res;
            }
            catch
            {
                MessageBox.Show("Unet JMBG nije u validnom formatu", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ""; 
                //throw new Exception("Unet JMBG nije u validnom formatu");
            }

        }

        private string CheckYears(char[] years)
        {
            try
            {
                int result = 0;
                string res = "";


                int year1 = int.Parse(years[0].ToString());
                int year2 = int.Parse(years[1].ToString());
                int year3 = int.Parse(years[2].ToString());

                result = year1 * 100 + year2 * 10 + year3;

                if (result < 100)
                {
                    result = result + 2000;
                    res = result.ToString();
                }
                else
                {
                    result = result + 1000;
                    res = result.ToString();
                }

                return res;
            }
            catch
            {
                MessageBox.Show("Unet JMBG nije u validnom formatu", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
                //throw new Exception("Unet JMBG nije u validnom formatu");
            }

        }

        private string CheckRegion(char[] regions)
        {
            try
            {
                string res = "";


                int region1 = int.Parse(regions[0].ToString());
                int region2 = int.Parse(regions[1].ToString());

                switch (region1)
                {
                    case 0:
                        {
                            switch (region2)
                            {
                                case 1:
                                    {
                                        res = "Stranac u Bosni";
                                        break;
                                    }
                                case 2:
                                    {
                                        res = "Stranac u Crnoj Gori";
                                        break;
                                    }
                                case 3:
                                    {
                                        res = "Stranac u Hrvatskoj";
                                        break;
                                    }
                                case 4:
                                    {
                                        res = "Stranac u Makedoniji";
                                        break;
                                    }
                                case 5:
                                    {
                                        res = "Stranac u Sloveniji";
                                        break;
                                    }
                                case 7:
                                    {
                                        res = "Stranac u Srbiji";
                                        break;
                                    }
                                case 8:
                                    {
                                        res = "Stranac u Vojvodini";
                                        break;
                                    }
                                case 9:
                                    {
                                        res = "Stranac na Kosovu i Metohiji";
                                        break;
                                    }
                                default:
                                    {
                                        res = "Unet kod regije nije validan";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 1:
                        {
                            switch (region2)
                            {
                                case 0:
                                    {
                                        res = "Banja Luka, Bosna i Hercegovina";
                                        break;
                                    }
                                case 1:
                                    {
                                        res = "Bihac, Bosna i Hercegovina";
                                        break;
                                    }
                                case 2:
                                    {
                                        res = "Doboj, Bosna i Hercegovina";
                                        break;
                                    }
                                case 3:
                                    {
                                        res = "Gorazde, Bosna i Hercegovina";
                                        break;
                                    }
                                case 4:
                                    {
                                        res = "Livno, Bosna i Hercegovina";
                                        break;
                                    }
                                case 5:
                                    {
                                        res = "Mostar, Bosna i Hercegovina";
                                        break;
                                    }
                                case 6:
                                    {
                                        res = "Prijedor, Bosna i Hercegovina";
                                        break;
                                    }
                                case 7:
                                    {
                                        res = "Sarajevo, Bosna i Hercegovina";
                                        break;
                                    }
                                case 9:
                                    {
                                        res = "Zenica, Bosna i Hercegovina";
                                        break;
                                    }
                                case 8:
                                    {
                                        res = "Tuzla, Bosna i Hercegovina";
                                        break;
                                    }
                                default:
                                    {
                                        res = "Unet kod regije nije validan";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 2:
                        {
                            switch (region2)
                            {
                                case 1:
                                    {
                                        res = "Podgorica, Crna Gora";
                                        break;
                                    }
                                case 6:
                                    {
                                        res = "Niksic, Crna Gora";
                                        break;
                                    }
                                case 9:
                                    {
                                        res = "Pljevlja, Crna Gora";
                                        break;
                                    }
                                default:
                                    {
                                        res = "Unet kod regije nije validan";
                                        break;
                                    }

                            }
                            break;
                        }
                    case 3:
                        {
                            switch (region2)
                            {
                                case 0:
                                    {
                                        res = "Osijek, Slavonija region, Hrvatska";
                                        break;
                                    }
                                case 1:
                                    {
                                        res = "Bjelovar, Virovitica, Koprivnica, Pakrac, Podravina region, Hrvatska";
                                        break;
                                    }
                                case 2:
                                    {
                                        res = "Varazdin, Medjumurje region, Hrvatska";
                                        break;
                                    }
                                case 3:
                                    {
                                        res = "Zagreb, Hrvatska";
                                        break;
                                    }
                                case 4:
                                    {
                                        res = "Karlovac, Hrvatska";
                                        break;
                                    }
                                case 5:
                                    {
                                        res = "Gospic, Lika region, Hrvatska";
                                        break;
                                    }
                                case 6:
                                    {
                                        res = "Rijeka, Pula, Istra i Primorje region, Hrvatska";
                                        break;
                                    }
                                case 7:
                                    {
                                        res = "Sisak, Banovina region, Hrvatska";
                                        break;
                                    }
                                case 9:
                                    {
                                        res = "Ostalo u Hrvatskoj";
                                        break;
                                    }
                                case 8:
                                    {
                                        res = "Split, Zadar, Dubrovnik, Dalmacija region, Hrvatska";
                                        break;
                                    }
                                default:
                                    {
                                        res = "Unet kod regije nije validan";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 4:
                        {
                            switch (region2)
                            {
                                case 1:
                                    {
                                        res = "Bitola, Makedonija";
                                        break;
                                    }
                                case 2:
                                    {
                                        res = "Kumanovo, Makedonija";
                                        break;
                                    }
                                case 3:
                                    {
                                        res = "Ohrid, Makedonija";
                                        break;
                                    }
                                case 4:
                                    {
                                        res = "Prilep, Makedonija";
                                        break;
                                    }
                                case 5:
                                    {
                                        res = "Skopje, Makedonija";
                                        break;
                                    }
                                case 6:
                                    {
                                        res = "Strumica, Makedonija";
                                        break;
                                    }
                                case 7:
                                    {
                                        res = "Tetovo, Makedonija";
                                        break;
                                    }
                                case 9:
                                    {
                                        res = "Stip, Makedonija";
                                        break;
                                    }
                                case 8:
                                    {
                                        res = "Veles, Makedonija";
                                        break;
                                    }
                                default:
                                    {
                                        res = "Unet kod regije nije validan";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 5:
                        {
                            switch (region2)
                            {
                                case 0:
                                    {
                                        res = "Slovenija";
                                        break;
                                    }
                                default:
                                    {
                                        res = "Unet kod regije nije validan";
                                        break;
                                    }

                            }
                            break;
                        }
                    case 7:
                        {
                            switch (region2)
                            {
                                case 1:
                                    {
                                        res = "Beograd region, Srbija";
                                        break;
                                    }
                                case 2:
                                    {
                                        res = "Sumadija, Srbija";
                                        break;
                                    }
                                case 3:
                                    {
                                        res = "Nis region, Srbija";
                                        break;
                                    }
                                case 4:
                                    {
                                        res = "Juzna Morava, Srbija";
                                        break;
                                    }
                                case 5:
                                    {
                                        res = "Zajecar, Srbija";
                                        break;
                                    }
                                case 6:
                                    {
                                        res = "Podunavlje, Srbija";
                                        break;
                                    }
                                case 7:
                                    {
                                        res = "Podrinje i Kolubara, Srbija";
                                        break;
                                    }
                                case 9:
                                    {
                                        res = "Uzice region, Srbija";
                                        break;
                                    }
                                case 8:
                                    {
                                        res = "Kraljevo region, Srbija";
                                        break;
                                    }
                                default:
                                    {
                                        res = "Unet kod regije nije validan";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 8:
                        {
                            switch (region2)
                            {
                                case 0:
                                    {
                                        res = "Novi Sad region, Vojvodina, Srbija";
                                        break;
                                    }
                                case 1:
                                    {
                                        res = "Sombor region, Vojvodina, Srbija";
                                        break;
                                    }
                                case 2:
                                    {
                                        res = "Subotica region, Vojvodina, Srbija";
                                        break;
                                    }
                                case 5:
                                    {
                                        res = "Zrenjanin region, Vojvodina, Srbija";
                                        break;
                                    }
                                case 6:
                                    {
                                        res = "Pancevo region, Vojvodina, Srbija";
                                        break;
                                    }
                                case 7:
                                    {
                                        res = "Kikinda region, Vojvodina, Srbija";
                                        break;
                                    }
                                case 9:
                                    {
                                        res = "Sremska Mitrovica region, Vojvodina, Srbija";
                                        break;
                                    }
                                case 8:
                                    {
                                        res = "Ruma region, Vojvodina, Srbija";
                                        break;
                                    }
                                default:
                                    {
                                        res = "Unet kod regije nije validan";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 9:
                        {
                            switch (region2)
                            {
                                case 1:
                                    {
                                        res = "Pristina region, Kosovo i Metohija, Srbija";
                                        break;
                                    }
                                case 2:
                                    {
                                        res = "Kosovska Mitrovica region, Kosovo i Metohija, Srbija";
                                        break;
                                    }
                                case 3:
                                    {
                                        res = "Pec region, Kosovo i Metohija, Srbija";
                                        break;
                                    }
                                case 4:
                                    {
                                        res = "Djakovica region, Kosovo i Metohija, Srbija";
                                        break;
                                    }
                                case 5:
                                    {
                                        res = "Prizren region, Kosovo i Metohija, Srbija";
                                        break;
                                    }
                                case 6:
                                    {
                                        res = "Kosovsko pomoravski okrug, Kosovo i Metohija, Srbija";
                                        break;
                                    }
                                default:
                                    {
                                        res = "Unet kod regije nije validan";
                                        break;
                                    }
                            }
                            break;
                        }
                }
                return res;
            }
            catch
            {
                MessageBox.Show("Unet JMBG nije u validnom formatu", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
                //throw new Exception("Unet JMBG nije u validnom formatu");
            }
        }

        private string CheckGender(char[] genders)
        {
            try
            {
                int result = 0;
                string res = "";


                int gender1 = int.Parse(genders[0].ToString());
                int gender2 = int.Parse(genders[1].ToString());
                int gender3 = int.Parse(genders[2].ToString());

                result = gender1 * 100 + gender2 * 10 + gender3;

                if (result < 500)
                {
                    res = "Muski";
                }
                else
                {
                    res = "Zenski";
                }

                return res;
            }
            catch
            {
                MessageBox.Show("Unet JMBG nije u validnom formatu", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
                //throw new Exception("Unet JMBG nije u validnom formatu");
            }
        }

        private bool CheckControlSum(char[] controls)
        {
            try
            {
                int A = int.Parse(controls[0].ToString()); // A
                int B = int.Parse(controls[1].ToString()); // B
                int C = int.Parse(controls[2].ToString()); // V
                int D = int.Parse(controls[3].ToString()); // G
                int E = int.Parse(controls[4].ToString()); // D
                int F = int.Parse(controls[5].ToString()); // Dj
                int G = int.Parse(controls[6].ToString()); // E
                int H = int.Parse(controls[7].ToString()); // Zh
                int I = int.Parse(controls[8].ToString()); // Z
                int J = int.Parse(controls[9].ToString()); // I
                int K = int.Parse(controls[10].ToString()); // J
                int L = int.Parse(controls[11].ToString()); // K
                int M = int.Parse(controls[12].ToString()); // L

                int sum = 11 - ((7 * (A + G) + 6 * (B + H) + 5 * (C + I) + 4 * (D + J) + 3 * (E + K) + 2 * (F + L)) % 11);

                if (sum > 9)
                {
                    sum = 0;
                }

                if (sum != M)
                {
                    MessageBox.Show("JMBG nije validan - kontrolne sume se ne poklapaju", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbxInput.Text = "";
                    return false;
                }
                return true;
            }
            catch
            {
                MessageBox.Show("Unet JMBG nije u validnom formatu", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnSumbit_Click(object sender, EventArgs e)
        {
            string temp = tbxInput.Text;

            if (temp.Length != 13)
            {
                MessageBox.Show("JMBG mora imati tacno 13 cifara", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbxInput.Text = "";
                return;
            }

            if (!CheckDate(temp.ToCharArray()))
            {
                return;
            }

            if (!CheckControlSum(temp.ToCharArray()))
            {
                return;
            }

            string days = temp.Substring(0, 2);
            string months = temp.Substring(2, 2);
            string years = temp.Substring(4, 3);

           

            tbxDate.Text = CheckDays(days.ToCharArray()) + "." + CheckMonths(months.ToCharArray()) + "." + CheckYears(years.ToCharArray()) + ".";

            tbxPlace.Text = CheckRegion(temp.Substring(7, 2).ToCharArray());

            tbxGender.Text = CheckGender(temp.Substring(9, 3).ToCharArray());
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tbxDate.Text = "";
            tbxGender.Text = "";
            tbxName.Text = "";
            tbxPlace.Text = "";
            tbxLastname.Text = "";
        }
    }
}
