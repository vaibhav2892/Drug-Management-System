using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;


namespace drugsample
{
	/// <summary>
	/// Summary description for clinical_condition_master.
	/// </summary>
	public partial class clinical_condition_master : System.Web.UI.Page
	{
		//SqlConnection cn=new SqlConnection(ConfigurationSettings.AppSettings["con"]);
        SqlConnection cn = new SqlConnection("Data Source=.; Initial Catalog=drugmanagement; Integrated Security=True");
		SqlCommand cmd;
		SqlDataAdapter da;
		SqlCommandBuilder cb;
		DataSet ds;
		DataRow r;
		int id;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if( Page.IsPostBack==false)
			{
				filldata();
			
				da=new SqlDataAdapter("select * from condition_master",cn);
				dr_cond_id.Items.Clear();
				da.Fill(ds,"cond_master");
				int i;
				int count;
				count= ds.Tables["cond_master"].Rows.Count;
				Array a=Array.CreateInstance( typeof(String), count, 0 );
				for (i=0;i<count;i++)
				{
					a=ds.Tables["cond_master"].Rows[i].ItemArray;
					dr_cond_id.Items.Add(a.GetValue(1).ToString());
					dr_cond_id.Items[dr_cond_id.Items.Count-1].Value=a.GetValue(0).ToString();
				}
			}

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.DataGrid1.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid1_PageIndexChanged);
			this.DataGrid1.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_CancelCommand);
			this.DataGrid1.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_EditCommand);
			this.DataGrid1.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_UpdateCommand);
			this.DataGrid1.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_DeleteCommand);

		}
		#endregion
//Filldata Method
		private void filldata()
		{
			ds=new DataSet();
			da=new SqlDataAdapter("select * from clinical_cond_master",cn);
			da.Fill(ds,"clinical_master");
			DataGrid1.DataSource=ds;
			DataGrid1.DataBind();
			
		}
// New Record Coding
		protected void but_new_Click(object sender, System.EventArgs e)
		{
			DataGrid1.Visible=false;
			Panel1.Visible=true;
			but_new.Visible=false;
			
			if(cn.State==ConnectionState.Open)
			{
				cn.Close();
                
			}
			cn.Open();
			
			cmd=new SqlCommand("select count(*) from clinical_cond_master",cn);
			id=Convert.ToInt32(cmd.ExecuteScalar());
			cn.Close();
			if(id==0)
			{
				txt_cli_id.Text="1";
			}
			else
			{
				cn.Open();
				cmd=new SqlCommand("select max(clinical_cond_id)+1 from clinical_cond_master",cn);
				txt_cli_id.Text=Convert.ToInt32(cmd.ExecuteScalar()).ToString();
				cn.Close();
			}
			txt_cli_name.Text="";
			txt_cli_comm.Text="";

			
		}

//Save Button Code
		protected void but_save_Click(object sender, System.EventArgs e)
		{
			filldata();
			r=ds.Tables["clinical_master"].NewRow();
			r[0]=Convert.ToInt32(txt_cli_id.Text);
			r[1]=txt_cli_name.Text.ToString();
			r[2]=txt_cli_comm.Text.ToString();
			r[3]=Convert.ToInt32(dr_cond_id.SelectedItem.Value);
			ds.Tables["clinical_master"].Rows.Add(r);
			cb=new SqlCommandBuilder(da);
			da.Update(ds,"clinical_master");
			filldata();
			DataGrid1.Visible=true;
			Panel1.Visible=false;
			but_new.Visible=true;
	
		}

// Back Grid Button Code
		protected void but_grid_Click(object sender, System.EventArgs e)
		{
			DataGrid1.Visible=true;
			Panel1.Visible=false;
			but_new.Visible=true;
		}

// Grid Edit Command
		private void DataGrid1_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataGrid1.EditItemIndex=e.Item.ItemIndex;
			filldata();
		}

//DataGrid Update Command Coding
		private void DataGrid1_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			filldata();
			TextBox a=(TextBox)e.Item .Cells [0].Controls [0];
			TextBox b=(TextBox)e.Item .Cells [1].Controls [0];
			TextBox c=(TextBox)e.Item .Cells [2].Controls [0];
			TextBox d=(TextBox)e.Item .Cells [3].Controls [0];
			DataRow r;
			int rownumber;
			rownumber=e.Item.ItemIndex;
			r=ds.Tables["clinical_master"].Rows[rownumber];
			r[0]= a.Text;
			r[1]= b.Text;
			r[2]= c.Text;
			r[3]= d.Text;
			cb=new SqlCommandBuilder(da);
			da.Update(ds,"clinical_master");
			DataGrid1.EditItemIndex=-1;
			filldata();
		
		}

	
//DataGrid1_CancelCommand
		private void DataGrid1_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataGrid1.EditItemIndex =-1;
			filldata();
		}

//DataGrid1_DeleteCommand
		private void DataGrid1_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			filldata();
			int a;
			a=e.Item.ItemIndex;
			ds.Tables["clinical_master"].Rows[a].Delete();
			cb=new SqlCommandBuilder(da);
			da.Update(ds,"clinical_master");
			filldata();
		}

		private void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			DataGrid1.CurrentPageIndex=e.NewPageIndex;
			filldata();
		}


        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
}
}
