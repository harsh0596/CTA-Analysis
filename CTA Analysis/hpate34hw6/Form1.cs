//
// CTA Ridership analysis using C# and SQL Serer.
//
// Harsh Patel
// U. of Illinois, Chicago
// CS341, Fall 2016
// Homework 6
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace hpate34hw6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // LOADS THE STATIONS ONTO LISTBOX1

            // INITIATE THE DATABASE CONNECTION
            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            // QUERY TO GET ALL THE STATIONS
            string sql = string.Format (
                @"select Name
                  from stations
                  order by Name ASC"
            );

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.CommandText = sql;
            adapter.Fill(ds);

            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                string msg = string.Format("{0}", row["Name"]);
                this.listBox1.Items.Add(msg);
            }

            // QUERY TO GET NUMBER OF STATIONS
            string sql2 = string.Format (
                @"select count(Name)
                  from stations"
            );

            cmd.CommandText = sql2;
            object result;
            result = cmd.ExecuteScalar();
            string msg2 = String.Format("Number of stations: {0}", result);
            textBox2.Text = msg2;

            // CLOSE THE DATABASE CONNECTION
            db.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            // % RIDERSHIP
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // CLEAR EVERYTHING
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();

            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            // TOTAL RIDERSHIP
            string userInput = this.listBox1.GetItemText(listBox1.SelectedItem);
            userInput = userInput.Replace("'", "''");
            string sql = string.Format(
                @"select sum(DailyTotal) as Total
                from Riderships
                join Stations
                on Riderships.StationID = Stations.StationID
                where Stations.Name = '{0}'", userInput
            );

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            object result;
            result = cmd.ExecuteScalar();
            string msg = String.Format("{0:n0}", result);
            string totalRidershipString = String.Format("{0}", result);
            double totalRidership = Convert.ToDouble(totalRidershipString);
            textBox5.Text = msg;

            // AVERAGE RIDERSHIP
            sql = string.Format(
                @"select avg(DailyTotal)
                from Riderships
                join Stations
                on Riderships.StationID = Stations.StationID
                and Stations.Name = '{0}'", userInput
            );

            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            msg = String.Format("{0:n0}/day", result);
            textBox6.Text = msg;

            // PERCENT RIDERSHIP
            sql = string.Format(
                @"select sum(Convert(bigint, DailyTotal))
                from Riderships
                join Stations
                on Riderships.StationID = Stations.StationID"
            );

            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            msg = String.Format("{0}", result);
            double total = Convert.ToDouble(msg);
            double avg = (totalRidership / total) * 100;
            string avgString = string.Format("{0:0.00}%", avg);
            textBox7.Text = avgString;

            // WEEKDAY
            sql = string.Format(
                @"select sum(DailyTotal)
                from Riderships
                join Stations
                on Stations.StationID = Riderships.StationID
                where Stations.Name = '{0}'
                and Riderships.TypeOfDay = 'W'", userInput
            );

            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            msg = String.Format("{0:n0}", result);
            textBox8.Text = msg;

            // SATURADY
            sql = string.Format(
                @"select sum(DailyTotal)
                from Riderships
                join Stations
                on Stations.StationID = Riderships.StationID
                where Stations.Name = '{0}'
                and Riderships.TypeOfDay = 'A'", userInput
            );

            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            msg = String.Format("{0:n0}", result);
            textBox9.Text = msg;

            // SUNDAY
            sql = string.Format(
                @"select sum(DailyTotal)
                from Riderships
                join Stations
                on Stations.StationID = Riderships.StationID
                where Stations.Name = '{0}'
                and Riderships.TypeOfDay = 'U'", userInput
            );

            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            msg = String.Format("{0:n0}", result);
            textBox10.Text = msg;


            // STOPS AT THIS STATION
            this.listBox2.Items.Clear();
            sql = string.Format(
                @"select Stops.Name as StopName
                from Stops
                join Stations
                on Stops.StationID = Stations.StationID
                where Stations.Name like '{0}'
                order by Stops.Name ASC", userInput
            );

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.CommandText = sql;
            adapter.Fill(ds);

            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                msg = string.Format("{0}", row["StopName"]);
                this.listBox2.Items.Add(msg);
            }

            // CLOSE THE DATABASE CONNECTION
            db.Close();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // CLEAR OUT BOXES CHANGED BY LISTBOX2
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            listBox3.Items.Clear();

            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            // HANDICAP
            string userInput = this.listBox2.GetItemText(listBox2.SelectedItem);
            userInput = userInput.Replace("'", "''");
            string sql = string.Format(
                @"select Stops.ADA
                from Stops
                join Stations
                on Stations.StationID = Stops.StationID
                where Stops.Name = '{0}'", userInput
            );

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            object result = cmd.ExecuteScalar();
            string msg = String.Format("{0}", result);
            if (msg.Equals("True"))
                this.textBox1.Text = "Yes";
            else
                this.textBox1.Text = "No";

            // DIRECTION
            sql = string.Format(
                @"select Stops.Direction
                from Stops
                join Stations
                on Stations.StationID = Stops.StationID
                where Stops.Name = '{0}'", userInput
            );

            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            msg = String.Format("{0}", result);
            this.textBox3.Text = msg;

            // LOCATION
            sql = string.Format(
                @"select Stops.Latitude
                from Stops
                join Stations
                on Stations.StationID = Stops.StationID
                where Stops.Name = '{0}'", userInput
            );

            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            string latitude = String.Format("{0:0.0000}", result);

            sql = string.Format(
                @"select Stops.Longitude
                from Stops
                join Stations
                on Stations.StationID = Stops.StationID
                where Stops.Name = '{0}'", userInput
            );

            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            string longitude = String.Format("{0:0.0000}", result);

            msg = string.Format("({0}, {1})", latitude, longitude);
            textBox4.Text = msg;

            // LINES
            sql = string.Format(
                @"select Lines.Color as Lines
                from Stations, Stops, StopDetails, Lines
                where Stations.StationID = Stops.StationID
                and Stops.StopID = StopDetails.StopID
                and StopDetails.LineID = Lines. LineID
                and Stops.Name = '{0}'", userInput
            );

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.CommandText = sql;
            adapter.Fill(ds);

            this.listBox3.Items.Clear();
            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                msg = string.Format("{0}", row["Lines"]);
                this.listBox3.Items.Add(msg);
            }

            db.Close();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // TOTAL RIDERSHIP
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // AVG RIDERSHIP
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            // WEEKDAY
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            // SATURDAY
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            // SUN/HOLIDAY
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // HANDICAP ACCESSIBLE
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // DIRECTION OF TRAVEL
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // LOCATION
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LINES
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // NUMBER OF STATIONS
        }

        private void top10StationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Top 10

            // CLEAR EVERYTHING
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();

            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            // QUERY TO GET TOP 10 STATIONS
            string sql = string.Format(
                @"select top 10 Stations.Name
                from Stations
                join Riderships
                on Stations.StationID = Riderships.StationID
                group by Name
                order by sum(Riderships.DailyTotal) DESC"
            );

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.Connection = db;
            cmd.CommandText = sql;
            adapter.Fill(ds);

            this.listBox1.Items.Clear();
            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                string msg = string.Format("{0}", row["Name"]);
                this.listBox1.Items.Add(msg);
            }

            textBox2.Text = "Number of station: 10";

            // CLOSE THE DATABASE CONNECTION
            db.Close();
        }
    }
}
