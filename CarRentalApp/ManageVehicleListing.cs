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
    public partial class ManageVehicleListing : Form
    {
        private readonly RentalEntities _db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            _db = new RentalEntities();  
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            //var cars = _db.TypeofCars.ToList();
            // var cars = _db.TypeofCars.Select(q => new { ID = q.Id, Name = q.Make }).ToList();
            // var cars = _db.TypeofCars.Select(q => new { q.Id,Make = q.Make, Model = q.model, VIN = q.VIN, Year = q.Year, LicensePlateNumber = q.LicencsePlateNumber }).ToList();
            //   gvVehicleList.DataSource = cars;
            //gvVehicleList.Columns[0].Visible = false;            
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Simple Refresh Option
            PopulateGrid();

        }
        public void PopulateGrid()
        {
            // Select a custom model collection of cars from database
            var cars = _db.TypeofCars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicencsePlateNumber,
                    q.Id
                })
                .ToList();
            gvVehicleList.DataSource = cars;
           
            //Hide the column for ID. Changed from the hard coded column value to the name, 
            // to make it more dynamic. 
            gvVehicleList.Columns["Id"].Visible = false;
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddEditVehicle"))
            {
                AddEditVehicle addEditVehicle = new AddEditVehicle(this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;

                var car = _db.TypeofCars.FirstOrDefault(q => q.Id == id);
                if (!Utils.FormIsOpen("AddEditVehicle"))
                {
                    var addEditVehicle = new AddEditVehicle(car,this);
                    addEditVehicle.MdiParent = this.MdiParent;
                    addEditVehicle.Show();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;
            var car = _db.TypeofCars.FirstOrDefault(q => q.Id == id);
            DialogResult dr = MessageBox.Show("Are You Sure You Want To Delete This Record?",
                    "Delete", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                _db.TypeofCars.Remove(car);
                _db.SaveChanges();
            }
            PopulateGrid();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
