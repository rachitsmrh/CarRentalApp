using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
  
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private readonly RentalEntities _db;
        private ManageVehicleListing _manageVehicleListing;
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
           BdTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            _db = new RentalEntities();
        }
        public AddEditVehicle(TypeofCar carToEdit, ManageVehicleListing manageVehicleListing =null)
        {
            InitializeComponent();
            BdTitle.Text = "Edit Vehicle";
            this.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;
            if (carToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new RentalEntities();
                PopulateFields(carToEdit);
            }
           
        }

        private void PopulateFields(TypeofCar car)
        {
            lblId.Text=car.Id.ToString();   
            tbMake.Text = car.Make;
            tbModel.Text = car.model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicense.Text = car.LicencsePlateNumber;
            
        }

        private void bdSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Added Validation for make and model
                if (string.IsNullOrWhiteSpace(tbMake.Text) ||
                        string.IsNullOrWhiteSpace(tbModel.Text))
                {
                    MessageBox.Show("Please ensure that you provide a make and a model");
                }
                else
                {
                    //if(isEditMode == true)
                    if (isEditMode)
                    {
                        //Edit Code here
                        var id = int.Parse(lblId.Text);
                        var car = _db.TypeofCars.FirstOrDefault(q => q.Id == id);
                        car.model = tbModel.Text;
                        car.Make = tbMake.Text;
                        car.VIN = tbVIN.Text;
                        car.Year = int.Parse(tbYear.Text);
                        car.LicencsePlateNumber = tbLicense.Text;


                    }
                    else
                    {
                        //Added validation for make and model of cars being added

                        // Add Code Here
                        var newCar = new TypeofCar
                        {
                            LicencsePlateNumber = tbLicense.Text,
                            Make = tbMake.Text,
                            model = tbModel.Text,
                            VIN = tbVIN.Text,
                            Year = int.Parse(tbYear.Text)
                        };

                        _db.TypeofCars.Add(newCar);

                    }
                    _db.SaveChanges();
                    _manageVehicleListing.PopulateGrid();
                    MessageBox.Show("Operation Completed. Refresh Grid To see Changes");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

           
        }

        private void bdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
