using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string selectedFilePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files|*.csv|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog.FileName;
                label1.Text = $"Selected File: {selectedFilePath}"; // Update label1 text

                // Display the data in the DataGridView
                DisplayDataInDataGridView();

                MessageBox.Show($"File selected: {selectedFilePath}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a CSV file first using Button 1.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(selectedFilePath);

                if (lines.Length > 1)
                {
                    // Extract header to get the subject names
                    string[] headers = lines[0].Split(',');

                    // Add a new column for student averages
                    lines[0] += ",Student Average";

                    // Skip the first line (header) and calculate averages for each student
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] values = lines[i].Split(',');

                        // Calculate the average for each student
                        double studentAverage = 0;
                        for (int j = 4; j < values.Length; j++) // Start from the 5th column (English)
                        {
                            if (double.TryParse(values[j], out double subjectValue))
                            {
                                studentAverage += subjectValue;
                            }
                            else
                            {
                                MessageBox.Show($"Row {i}, Column {headers[j]} does not contain a valid numeric value.");
                                return; // Stop processing if a row has invalid values
                            }
                        }

                        studentAverage /= (values.Length - 4); // Exclude the ID, Name, Grade, Section columns
                        lines[i] += $",{studentAverage:F2}";
                    }

                    // Write the modified lines back to the file
                    File.WriteAllLines(selectedFilePath, lines);
                    MessageBox.Show("Student averages have been added.");

                    // Display the updated data in the DataGridView
                    DisplayDataInDataGridView();
                }
                else
                {
                    MessageBox.Show("CSV file is empty or does not contain enough rows for calculation.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void DisplayDataInDataGridView()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Load data from the CSV file to the DataGridView
            string[] lines = File.ReadAllLines(selectedFilePath);

            if (lines.Length > 0)
            {
                // Add columns based on header
                string[] headers = lines[0].Split(',');
                foreach (var header in headers)
                {
                    dataGridView1.Columns.Add(header, header);
                }

                // Add rows to the DataGridView
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] values = lines[i].Split(',');
                    dataGridView1.Rows.Add(values);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Your code for label1_Click event
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Your code for dataGridView1_CellContentClick event
        }
    }
}
