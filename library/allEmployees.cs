﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using library.classes;

namespace library
{
    public partial class allEmployees : Form
    {
        public List<string> errors = new List<string>();

        public List<string> accessLevels = new List<string>();

        public List<DataRow> employees = new List<DataRow>();

        public int employeesCount;

        CMySql DB = new CMySql();

        public List<string> columns = new List<string>() { "id", "login", "password", "name", "surname", "lastname", "dateOfRegister", "phone", "street", "building", "apartments", "accessLevels_id" };

        bool firstHit = true;

        public int formHeight;
        public allEmployees()
        {
            InitializeComponent();
            formHeight = this.Height;
            this.employeesCount = 0;
            getEmployees();
            firstHit = false;
        }

        public void getAllAccessLevels()
        {
            DB.execSelect("SELECT * FROM `accesslevels`;").ForEach(delegate (DataRow row)
            {
                accessLevels.Add(row["access"].ToString());
            });
        }

        public void getEmployees()
        {
            employees = DB.execSelect("SELECT * FROM library.users JOIN `accesslevels` ON `users`.`accessLevels_id` = `accesslevels`.`id`;");
            if (employees.Count > 0)
            {
                dataGridView1.RowCount = employees.Count;
                employees.ForEach(delegate (DataRow row) { setTable(row); });
            }
        }

        public void setTable(DataRow row)
        {
            dataGridView1.Rows[this.employeesCount].Cells[0].Value = Convert.ToString(row["id"]);
            dataGridView1.Rows[this.employeesCount].Cells[1].Value = Convert.ToString(row["login"]);
            dataGridView1.Rows[this.employeesCount].Cells[2].Value = Convert.ToString(row["password"]);
            dataGridView1.Rows[this.employeesCount].Cells[3].Value = Convert.ToString(row["name"]);
            dataGridView1.Rows[this.employeesCount].Cells[4].Value = Convert.ToString(row["surname"]);
            dataGridView1.Rows[this.employeesCount].Cells[5].Value = Convert.ToString(row["lastname"]);
            dataGridView1.Rows[this.employeesCount].Cells[6].Value = Convert.ToString(row["dateOfRegister"]);
            dataGridView1.Rows[this.employeesCount].Cells[7].Value = Convert.ToString(row["phone"]);
            dataGridView1.Rows[this.employeesCount].Cells[8].Value = Convert.ToString(row["street"]);
            dataGridView1.Rows[this.employeesCount].Cells[9].Value = Convert.ToString(row["building"]);
            dataGridView1.Rows[this.employeesCount].Cells[10].Value = Convert.ToString(row["apartments"]);
            dataGridView1.Rows[this.employeesCount].Cells[11].Value = Convert.ToString(row["access"]);
            this.employeesCount++;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (firstHit)
            {
                return;
            }
            resetForm();
            int row = Convert.ToInt32(e.RowIndex);
            int column = Convert.ToInt32(e.ColumnIndex);
            string fieldName = "";
            if (row > -1 && column > -1)
            {
                string newText = dataGridView1.Rows[row].Cells[column].Value.ToString();
                int id = Convert.ToInt32(dataGridView1.Rows[row].Cells[0].Value);
                if(column == 2)
                {
                    if(!CValidate.passwordValidate(newText))
                    {
                        errors.Add("Длина пароля должна быть больше 6 и пароль \nдолжен содержать заглавные буквы, цифры и занки препинания");
                        return;
                    }
                    newText = CValidate.preparePassword(newText);
                }
                if (column == 3 || column == 4 || column == 5)
                {
                    if (!CValidate.nameValidate(newText))
                    {
                        errors.Add("Фамилия не может содержать числа");
                    }
                }
                if(column == 1)
                {
                    if (!CValidate.loginValidate(newText))
                    {
                        errors.Add("Логин должен быть короче 45 символов, но длинее 1 символа");
                    }
                }
                if(column == 7)
                {
                    if (!CValidate.phoneValidate(newText))
                    {
                        errors.Add("Некоректный номер телефона");
                    }
                }
                if (column == 0)
                {
                    errors.Add("Нельзя поменять id записи!");
                    return;
                }
                if (column == 11)
                {
                    //Уровень доступа
                    if(newText == "B")
                        newText = "2";
                    if(newText == "A")
                        newText = "1";
                }
                fieldName = columns[column];
                if (errors.Count > 0)
                {
                    CErrors.showErrors(errorsLabel, this, errors);
                    return;
                }
                try
                {
                    DB.execUpdate($"UPDATE `users` SET `{fieldName}`='{newText}' WHERE `id` = {id};");
                }
                catch
                {
                    errors.Add("Не удалось обновить запись. Попробуйте позже!");
                }
            }
        }
        public void resetForm()
        {
            this.Height = formHeight;
            errors.Clear();
            errorsLabel.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
