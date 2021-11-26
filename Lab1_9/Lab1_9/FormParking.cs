﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DegtyarevBus
{
    public partial class FormParking : Form
    {
        private readonly ParkingCollection parkingCollection;
        public FormParking()
        {
            InitializeComponent();
            parkingCollection = new ParkingCollection(pictureBoxParking.Width,
            pictureBoxParking.Height);
        }

        private void ReloadLevels()
        {
            int index = listBoxParkings.SelectedIndex;
            listBoxParkings.Items.Clear();
            for (int i = 0; i < parkingCollection.Keys.Count; i++)
            {
                listBoxParkings.Items.Add(parkingCollection.Keys[i]);
            }
            if (listBoxParkings.Items.Count > 0 && (index == -1 || index >=
            listBoxParkings.Items.Count))
            {
                listBoxParkings.SelectedIndex = 0;
            }
            else if (listBoxParkings.Items.Count > 0 && index > -1 && index <
            listBoxParkings.Items.Count)
            {
                listBoxParkings.SelectedIndex = index;
            }
        }

        private void Draw()
        {
            if (listBoxParkings.SelectedIndex > -1)
            {
                Bitmap bmp = new Bitmap(pictureBoxParking.Width,
                pictureBoxParking.Height);
                Graphics gr = Graphics.FromImage(bmp);
                parkingCollection[listBoxParkings.SelectedItem.ToString()].Draw(gr);
                pictureBoxParking.Image = bmp;
            }
            else if (listBoxParkings.Items.Count == 0)
            {
                Bitmap bmp = new Bitmap(pictureBoxParking.Width, pictureBoxParking.Height);
                Graphics gr = Graphics.FromImage(bmp);
                pictureBoxParking.Image = bmp;
            }
        }


        private void buttonAddParking_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxNewLevelName.Text))
            {
                MessageBox.Show("Введите название парковки", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            parkingCollection.AddParking(textBoxNewLevelName.Text);
            ReloadLevels();
        }

        private void buttonDelParking_Click(object sender, EventArgs e)
        {

            if (listBoxParkings.SelectedIndex > -1)
            {
                if (MessageBox.Show($"Удалить построение { listBoxParkings.SelectedItem.ToString()}?",
                    "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    parkingCollection.DelParking(listBoxParkings.SelectedItem.ToString());
                    ReloadLevels();
                }
            }
            else
            {
                MessageBox.Show("Добавьте парковку", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSetBus_Click(object sender, EventArgs e)
        {
            if (listBoxParkings.SelectedIndex > -1)
            {
                var formBusConfig = new FormBusConfig();
                formBusConfig.AddEvent(AddBus);
                formBusConfig.Show();
            }
        }

        private void AddBus(Vehicle bus)
        {
            if (bus != null && listBoxParkings.SelectedIndex > -1)
            {
                if (((parkingCollection[listBoxParkings.SelectedItem.ToString()]) + bus) != -1)
                {
                    Draw();
                }
                else
                {
                    MessageBox.Show("Автобус не удалось поставить");
                }
            }
            else
            {
                MessageBox.Show("Добавьте парковку", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonTakeBus_Click(object sender, EventArgs e)
        {
            if (listBoxParkings.SelectedIndex > -1 && maskedTextBox.Text != "")
            {
                var bus = parkingCollection[listBoxParkings.SelectedItem.ToString()] -
                Convert.ToInt32(maskedTextBox.Text);
                if (bus != null)
                {
                    BusForm form = new BusForm();
                    form.SetBus(bus);
                    form.ShowDialog();
                }
                Draw();
            }
        }

        private void listBoxParkings_SelectedIndexChanged(object sender, EventArgs e)
        {
            Draw();
        }
    }
}
