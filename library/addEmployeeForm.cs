﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using library.classes;

namespace library
{
    public partial class addEmployeeForm : Form
    {
        public List<string> errors = new List<string>();

        CMySql DB = new CMySql();

        public int formHeight;
        public addEmployeeForm()
        {
            InitializeComponent();
            formHeight = this.Height;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resetForm();
            DB.addNewUser(ref errors, loginTextBox.Text, passwordTextBox.Text, nameTextBox.Text, surnameTextBox.Text, lastNameTextBox.Text, phoneNumTextBox.Text, streetTextBox.Text, buildingTextBox.Text, apartmentsTextBox.Text, '2');
            if(errors.Count > 0)
            {
                CErrors.showErrors(errorsLabel, this, errors);
            }else
            {
                successLabel.Text = "Сотрудник успешно добавлен!";
            }
        }

        public void resetForm()
        {
            errorsLabel.Text = "";
            this.Height = formHeight;
            errors.Clear();
        }
    }
}
