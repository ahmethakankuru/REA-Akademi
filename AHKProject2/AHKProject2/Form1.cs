using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHKProject2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection;
        SqlDataAdapter adapter;
        DataSet ds;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // satır ve sütünların indexleri geçerli mi
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString(); // tıklanan her hücreyi id den itibaren toStringe çevirir
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["isim"].Value.ToString(); /// ilgili tecxtboxa yerleştir
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["soyad"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["telefon"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["adres"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getir();
        }

        private void Form1_load(object sender, EventArgs e)
        {
            getir();
        }


        private void getir()
        {
            connection = new SqlConnection("server=DESKTOP-HSHKVJO; Initial Catalog=okul; Integrated Security=true");
            adapter = new SqlDataAdapter("select * from Ogrenci", connection);
            ds = new DataSet();
            connection.Open();
            adapter.Fill(ds, "Ogrenci");
            dataGridView1.DataSource = ds.Tables["Ogrenci"];
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string yeniId = textBox1.Text; 
            string yeniIsim = textBox2.Text;
            string yeniSoyisim = textBox3.Text;
            string yeniTelefon = textBox4.Text;
            string yeniAdres = textBox5.Text;

            connection.Open();
            SqlCommand command = new SqlCommand("INSERT INTO Ogrenci (id, isim, soyad, telefon, adres) VALUES (@id, @isim, @soyad, @telefon, @adres)", connection);
            command.Parameters.AddWithValue("@id", yeniId);
            command.Parameters.AddWithValue("@isim", yeniIsim);
            command.Parameters.AddWithValue("@soyad", yeniSoyisim);  // ekleye basınca her sütundaki veriyi alır gönderir
            command.Parameters.AddWithValue("@telefon", yeniTelefon);
            command.Parameters.AddWithValue("@adres", yeniAdres);
            command.ExecuteNonQuery(); // sorguyu veri tabanına gönderiyor insert into delete için gerekli
            connection.Close();

            MessageBox.Show("Yeni veri eklendi.");
            getir(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text)) //  boxun boş olup olmadığını bakıyor 
            {
                int guncellenecekId = Convert.ToInt32(textBox1.Text); // box içindeki veriyi integer a çevirir
                //toInt32 bir değeri integer a dönüştürür
                string yeniIsim = textBox2.Text;
                string yeniSoyisim = textBox3.Text;
                string yeniTelefon = textBox4.Text;
                string yeniAdres = textBox5.Text;

                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Ogrenci SET isim = @isim, soyad = @soyad, telefon = @telefon, adres = @adres WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", guncellenecekId); // id parametresine göre güncelleme değeri atar o id ye ait olanı günceller
                command.Parameters.AddWithValue("@isim", yeniIsim);
                command.Parameters.AddWithValue("@soyad", yeniSoyisim);
                command.Parameters.AddWithValue("@telefon", yeniTelefon);
                command.Parameters.AddWithValue("@adres", yeniAdres);
                int affectedRows = command.ExecuteNonQuery();//güncellemeyi veritabanına gönderir
                connection.Close();

                if (affectedRows > 0) // değişken değerine bakıyor en az bir verinin etkilenmesine bakıyor
                {
                    MessageBox.Show("Veri güncellendi.");
                    getir(); 
                }
                else
                {
                    MessageBox.Show("Veri güncellenemedi.");
                }
            }
            else
            {
                MessageBox.Show("Güncellenecek verinin ID'sini girin.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // en az bir veri seti seçilmiş ise 
            {
                int guncellenecekId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                //seçilen satırda id değerini alı ve integer a döner silinecek değeri temsil ediyor

                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Ogrenci WHERE id = @id", connection);
                // belirli ıd ye sahip olan seri setini siler burdaki parametre id dir
                command.Parameters.AddWithValue("@id", guncellenecekId);
                //silinecek verinin id değeri belirlenir  addwithvalue ile soğru veri setine dönüşüm yapar

                int affectedRows = command.ExecuteNonQuery(); // veri tabanına sorguyu gönderiyor
                connection.Close();

                if (affectedRows > 0)
                {
                    MessageBox.Show("Veri silindi.");
                    getir(); 
                }
                else
                {
                    MessageBox.Show("Veri silinemedi.");
                }
            }
            else
            {
                MessageBox.Show("Silmek için bir satır seçin.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
    }
}
