using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Drawing.Drawing2D;

namespace MagiBazaSQLITE
{
    public partial class Glavna : Form
    {
       

        public Glavna()
        {
            InitializeComponent();
            
           
            
        }


        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private SQLiteDataAdapter DB;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            txtDatum.CustomFormat = "dd.MM.yyyy";
            txtDatum.Format = DateTimePickerFormat.Custom;
            txtRadnik.Text = "Rade Pralica";
            txtBrojOperacija.Text = "1";
            
           
            LoadData();
  dgrElta.Columns[9].Visible = false;

            //this.dgrElta.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
            //this.dgrElta.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
             
        }

        private void UkupnoStavki()
        {
            //int broj = dgrElta.RowCount;
            //lblUkupno.Text = (broj - 1).ToString();
int sum = 0;

 for (int i = 0; i < dgrElta.Rows.Count; i++)
            {
                sum += Convert.ToInt32(dgrElta.Rows[i].Cells[8].Value);
            }

            
            if (sum<=59)
            {
                lblUkupno.Text = sum.ToString() +" Minuta";
            }
            else if (sum>59)
            {
               
            lblUkupno.Text = sum / 60 + ":" + (sum % 60).ToString() + " Sati";

            }
           
            
        }

        private void SetConnection()
        {
            sql_con = new SQLiteConnection("Data Source=Pripravnost.db;Version=3;New=False;Compress=true;");
        }
        //Set Execute querry

        private void ExecuteQuery(string txtQuery)
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }

        //Load Database

        private void LoadData()
        {

            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT * FROM Rad";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            dgrElta.DataSource = DT;
            sql_con.Close();
            UkupnoStavki();
        }


        private void btnOcisti_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            txtKorisnik.Text = "";
            txtKolega.Text = "";
            txtProblem.Text = "";
            txtRadnja.Text = "";
            txtKomentar.Text = "";
            txtBrojOperacija.Text = "";
            txtVrijeme.Text = "";
            txtVrijeme.Text = "";
        }
      
        private void btnDodaj_Click(object sender, EventArgs e)
        {

            if (txtKorisnik.Text ==""  )
            {
                MessageBox.Show("Morate Upisati Korisnika", "Upozorenje");
            }
            else if(txtRadnik.Text == "")
            {
                MessageBox.Show("Morate Odabrati radnika", "Upozorenje");
            }
            
            else if(txtRadnja.Text == "")
            {
                MessageBox.Show("Morate Upisati Radnju", "Upozorenje");
            }
            else if (txtVrijeme.Text=="")
            {
                MessageBox.Show("Morate selektovati vrijeme trajanja", "Upozorenje");
            }

            else
            {
String txtQuery = "INSERT INTO Rad ([Korisnik/Kds],[Problem],[Radnja],[Kolega sa Terena]," +
                "[Komentar],[Broj Operacija],[Datum],[Radnik],[Vrijeme])" +
                "Values('" + txtKorisnik.Text + "','"+txtProblem.Text+ "','" + txtRadnja.Text + "','" + txtKolega.Text + "'," +
                "'" + txtKomentar.Text + "','" + txtBrojOperacija.Text + "','" + txtDatum.Text + "','" + txtRadnik.Text + "'" +
                ",'" + txtVrijeme.Text + "')";
            ExecuteQuery(txtQuery);
            LoadData();
            Clear();
            txtKorisnik.Focus();
            }
            

        }
       

        private void btnIzbrisi_Click(object sender, EventArgs e)
        {
           
            string query = "DELETE FROM Rad WHERE Id='" + txtId.Text + "'";
            ExecuteQuery(query);
            LoadData();
    
        }

       

        private void btnPrepravi_Click(object sender, EventArgs e)
        {
            string txtQuery = "UPDATE Rad SET [Korisnik/Kds]='" + txtKorisnik.Text + "',[Problem]='" + txtProblem.Text + "'," +
              "[Radnja]='" + txtRadnja.Text + "',[Kolega sa Terena]='" + txtKolega.Text + "', "+
            "[Komentar]='" + txtKomentar.Text + "',[Broj Operacija]='" + txtBrojOperacija.Text + "',[Datum]='" + txtDatum.Text + "', "+
            "[Radnik]='" + txtRadnik.Text + "',[Vrijeme]='" + txtVrijeme.Text + "' WHERE ID='" + txtId.Text + "'" ;

           ExecuteQuery(txtQuery);

            LoadData();
        }

        
private void btnObrisiTabelu_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Jeste li sigurni da želite obrisati podatke ?", "Brisanje tabele ?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
            string query = "DELETE  FROM Rad";
            ExecuteQuery(query);

            LoadData();
            }
            
            else
            {
                return;
            }
           
        }
       

        

        
        private void SetProizvidiGrid()
        {
            
            dgrElta.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgrElta.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
           
            dgrElta.AutoResizeColumns();
            dgrElta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgrElta.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
           
            dgrElta.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 9, FontStyle.Bold);
            dgrElta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgrElta.DefaultCellStyle.Font = new Font("Arial", 9.5F, FontStyle.Bold, GraphicsUnit.Pixel);

        }

        

       

        private void dgrElta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            txtKorisnik.Text = dgrElta.SelectedRows[0].Cells[0].Value.ToString();
            txtProblem.Text = dgrElta.SelectedRows[0].Cells[1].Value.ToString();
            txtRadnja.Text = dgrElta.SelectedRows[0].Cells[2].Value.ToString();
            txtKolega.Text = dgrElta.SelectedRows[0].Cells[3].Value.ToString();
            txtKomentar.Text = dgrElta.SelectedRows[0].Cells[4].Value.ToString();
            txtBrojOperacija.Text = dgrElta.SelectedRows[0].Cells[5].Value.ToString();
            //txtDatum.Text = dgrElta.SelectedRows[0].Cells[6].Value.ToString();
            txtRadnik.Text = dgrElta.SelectedRows[0].Cells[7].Value.ToString();
           txtVrijeme.Text = dgrElta.SelectedRows[0].Cells[8].Value.ToString();
         txtId.Text = dgrElta.SelectedRows[0].Cells[9].Value.ToString();
        } 

       

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            for (int i = 1; i < dgrElta.Columns.Count  ; i++)
            {
                worksheet.Cells[1, i] = dgrElta.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < dgrElta.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgrElta.Columns.Count -1; j++)
                {
                    if (dgrElta.Rows[i].Cells[j].Value != null)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgrElta.Rows[i].Cells[j].Value.ToString();
                    }
                    else
                    {
                        worksheet.Cells[i + 2, j + 1] = "";
                    }
                }
            }
        }

        //Redni brojevi u datagridview
        private void dgrElta_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void Glavna_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraphics = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 155, 173), 1);
            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(96, 155, 173), Color.FromArgb(245, 251, 251), LinearGradientMode.Vertical);
            mgraphics.FillRectangle(lgb, area);
            mgraphics.DrawRectangle(pen, area);
        }

       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnUpis_MouseHover(object sender, EventArgs e)
        {
            for (int i = 0; i < 50; i++)
            {
                txtBrojOperacija.Items.Add(i);
            }


            txtBrojOperacija.Text = "1";
            txtKorisnik.Focus();
            txtRadnik.Text = "Rade Pralica";

            
        }

       

        private void btnUpis_Click(object sender, EventArgs e)
        {
            //if (Hided)
            //{
            //    btnUpis.Text = "Hide";
            //    Clear();
            //    timer1.Start();
            //}
            //else
            //{
            //    btnUpis.Text = "Upis";
            //    timer1.Start();
            //}
        }

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Spanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gradientPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}

       
    
    

