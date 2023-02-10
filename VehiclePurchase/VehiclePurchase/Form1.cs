/*
 * Author: Samuel Greenlee
 * Project: Assignment Two
 * Date  : January 30, 2020
 * Desc  : Calculates the price of a car
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehiclePurchase
{
    public partial class frmCenterOfCars : Form
    {
        public frmCenterOfCars()
        {
            InitializeComponent();
        }

        private void frmCenterOfCars_Load(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Today;
            lblDate.Text = currentDate.ToLongDateString(); 
            Reset();
        }

        private void btnTotal_Click(object sender, EventArgs e)
        {
            // Declaire variables
            decimal salePrice;
            decimal accessoriesAmount;
            decimal finishAmount;
            decimal totalFinishAccessoriesAmount;
            decimal subtotal;
            decimal salesTax;
            decimal total;
            decimal tradeInAllowance;
            decimal rangeCheck;
            decimal amountCheck;

            if (DataValid() == true) // Initiates data validation
            {
                // Get sale price
                salePrice = GetSalePrice(out salePrice);

                // Calculate the accessories and finish
                accessoriesAmount = CalculateAccessories();

                finishAmount = CalculateFinishAmount();

                totalFinishAccessoriesAmount = CalculateTotalFinishAccessoriesAmount(out totalFinishAccessoriesAmount, accessoriesAmount, finishAmount);

                // Calculate subtotal
                subtotal = CalculateSubtotal(out subtotal, salePrice, totalFinishAccessoriesAmount);

                // Calculate sales tax (8%)
                salesTax = CalculateSalesTax(out salesTax, subtotal);

                // Calculate total
                total = CalculateTotal(out total, subtotal, salesTax);

                // Get the trade-in allowance
                tradeInAllowance = GetTradeInAllowance(out tradeInAllowance);

                // Bool the trade-in allowance
                TradeInAllowanceConfiguration(tradeInAllowance, total);

                // Data validation check on sale price
                rangeCheck = Convert.ToDecimal(txtSalePrice.Text);

                // Data validation check on trade-in allowance
                amountCheck = Convert.ToDecimal(txtTradeInAllowance.Text);

                // This method displays the results needed in the read-only textboxes EXCEPT AMOUNT DUE
                Display(totalFinishAccessoriesAmount, subtotal, salesTax, total);
            }

        }

        private decimal GetSalePrice(out decimal salePrice)
        {
            salePrice = Convert.ToDecimal(txtSalePrice.Text);

            return salePrice;
        }


        private decimal CalculateAccessories()
        {
            decimal price = 0.00m;

            if (chkStereoSystem.Checked)
            {
                price += 425.76m;
            }

            if (chkLeatherInterior.Checked)
            {
                price += 987.41m;
            }

            if (chkGPSNavigation.Checked)
            {
                price += 1741.23m;
            }

            return price;
        }

        private decimal CalculateFinishAmount()
        {
            decimal price = 0.00m;

            if (rdoStandard.Checked == true)
            {
                price = 0.00m;
            }

            if (rdoPearl.Checked == true)
            {
                price = 345.72m;
            }

            if (rdoCustomizedDetailing.Checked == true)
            {
                price = 599.99m;
            }

            return price;
        }

        private decimal CalculateTotalFinishAccessoriesAmount(out decimal totalFinishAccessoriesAmount, decimal accessoriesAmount, decimal finishAmount)
        {
            totalFinishAccessoriesAmount = accessoriesAmount + finishAmount;

            return totalFinishAccessoriesAmount;

        }

        private decimal CalculateSubtotal(out decimal subtotal, decimal salePrice, decimal totalFinishAccessoriesAmount)
        {
            subtotal = salePrice + totalFinishAccessoriesAmount;

            return subtotal;
        }

        private decimal CalculateSalesTax(out decimal salesTax, decimal subtotal)
        {
            salesTax = subtotal * 0.08m;

            return salesTax;
        }

        private decimal CalculateTotal(out decimal total, decimal subtotal, decimal salesTax)
        {
            total = subtotal + salesTax;

            return total;
        }

        private decimal GetTradeInAllowance(out decimal tradeInAllowance)
        {
            tradeInAllowance = Convert.ToDecimal(txtTradeInAllowance.Text);

            return tradeInAllowance;
        }

        private bool TradeInAllowanceConfiguration(decimal tradeInAllowance, decimal total)
        {
            if (tradeInAllowance > 0)
            {
                total -= tradeInAllowance;

                // This will display the amount due in currency, if the trade in allowance is greater than zero

                txtAmountDue.Text = (total.ToString("f2")); // This rounds it to two decimal places

                txtAmountDue.Text = total.ToString("c"); // This displays it in currancy 

                return true;
            }
            return false;
        }

        // This is the methods containing data validation

        private bool DataValid()
        {
            return
                IsWhithInRange(txtSalePrice, "Sale price", 500, 30000) &&
                CheckOnAmount(txtTradeInAllowance, "Trade-in allowance", txtSalePrice);
        }

        private bool IsWhithInRange(TextBox textBox, string name, decimal min, decimal max)
        {
            decimal number = Convert.ToDecimal(textBox.Text);
            if (number < min || number > max) // || means or && means a conditional and
            {
                MessageBox.Show(name + " must be between " + min + " and " + max);
                textBox.Focus();
                textBox.SelectAll();
                return false;

            }
            else
            {
                return true;
            }
        }

        private bool CheckOnAmount(TextBox txtTradeInAllowance, string name, TextBox txtSalePrice)
        {
            decimal allowance = Convert.ToDecimal(txtTradeInAllowance.Text);
            decimal priceOfSale = Convert.ToDecimal(txtSalePrice.Text);
            if (allowance > priceOfSale)
            {
                MessageBox.Show(name + " must be less than the sale price");
                txtTradeInAllowance.Focus();
                txtTradeInAllowance.SelectAll();
                return false;
            }
            else
            {
                return true;
            }
        }

        // This method displays all the data in the read-only textboxes

        public void Display(decimal totalFinishAccessoriesAmount, decimal subtotal, decimal salesTax, decimal total)
        {
            // Displays accessories and finish               
            txtAAndFinish.Text = (totalFinishAccessoriesAmount.ToString("f2")); // This rounds the to  two decimal places

            // Displays subtotal 
            txtSubtotal.Text = (subtotal.ToString("f2")); // This rounds the to two decimal places

            // Displays sales tax (8%)
            txtSalesTax.Text = (salesTax.ToString("f2")); // This rounds the to two decimal places

            // Displays total
            txtTotal.Text = (total.ToString("f2")); // This rounds the to two decimal places

            txtTotal.Text = (total.ToString("c")); // By codeing it this way, it displays as currency
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            // Checked boxes

            chkStereoSystem.Checked = false;
            chkLeatherInterior.Checked = false;
            chkGPSNavigation.Checked = false;

            // Radio buttons

            rdoStandard.Checked = true;
            rdoPearl.Checked = false;
            rdoCustomizedDetailing.Checked = false;

            // Data in textboxes

            txtSalePrice.Clear();
            txtAAndFinish.Clear();
            txtSubtotal.Clear();
            txtSalesTax.Clear();
            txtTotal.Clear();
            txtTradeInAllowance.Clear();
            txtAmountDue.Clear();

            // Returns focus to the sale price textbox

            txtSalePrice.Focus();
        }

        // Exit Button

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
