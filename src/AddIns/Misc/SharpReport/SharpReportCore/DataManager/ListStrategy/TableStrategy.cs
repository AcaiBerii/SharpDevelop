//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Collections;
using System.Data;
using System.ComponentModel;

	
/// <summary>
/// This class handles DataTables
/// </summary>
/// <remarks>
/// 	created by - Forstmeier Peter
/// 	created on - 23.10.2005 15:12:06///
///</remarks>

namespace SharpReportCore {
	public class TableStrategy : BaseListStrategy {
		
		DataTable table;
		DataView view = new DataView();
		DataRowView row;
		
		
		public TableStrategy(DataTable table,ReportSettings reportSettings):base(reportSettings) {
			if (table == null) {
				throw new ArgumentNullException("table");
			}
			this.table = table;
			view = this.table.DefaultView;
		}
		
		
		
		#region Building the Index list
		
		private  void BuildSortIndex(SharpIndexCollection arrayList,ColumnCollection col) {
			
			try {
				for (int rowIndex = 0; rowIndex < this.view.Count; rowIndex++){
					DataRowView rowItem = this.view[rowIndex];
					object[] values = new object[col.Count];
					for (int criteriaIndex = 0; criteriaIndex < col.Count; criteriaIndex++){
						AbstractColumn c = (AbstractColumn)col[criteriaIndex];
						object value = rowItem[c.ColumnName];

						if (value != null && value != DBNull.Value){
							if (!(value is IComparable)){
								throw new InvalidOperationException("ReportDataSource:BuildSortArray - > This type doesn't support IComparable." + value.ToString());
							}
							
							values[criteriaIndex] = value;
						}   else {
							values[criteriaIndex] = DBNull.Value;
						}
					}
					arrayList.Add(new SortComparer(col, rowIndex, values));
				}
			} catch (Exception) {
				throw;
			}

			arrayList.Sort();
		}
	
		// if we have no sorting, we build the indexlist as well, so we don't need to
		//check each time we reasd data if we have to go directly or by IndexList
		private  void BuildPlainIndex(SharpIndexCollection arrayList,ColumnCollection col) {
			try {
				for (int rowIndex = 0; rowIndex < this.view.Count; rowIndex++){
					object[] values = new object[1];
					
					// We insert only the RowNr as a dummy value
					values[0] = rowIndex;
					arrayList.Add(new BaseComparer(col, rowIndex, values));
				}
			} catch (Exception) {
				throw ;
			}
		}
		
	
		#endregion
		
		#region Grouping
		
		private void BuildGroup(){
			try {
				SharpIndexCollection groupedArray = new SharpIndexCollection();
				
				if (base.ReportSettings.GroupColumnsCollection != null) {
					if (base.ReportSettings.GroupColumnsCollection.Count > 0) {
						this.BuildSortIndex (groupedArray,base.ReportSettings.GroupColumnsCollection);
					}
				}

				base.MakeGroupedIndexList (groupedArray);

//				System.Console.WriteLine("GroupedList with {0} elements",base.IndexList.Count);
			/*
				foreach (BaseComparer bc in this.IndexList) {
					GroupSeperator gs = bc as GroupSeperator;
					
					if (gs != null) {
//						System.Console.WriteLine("Group Header <{0}> with <{1}> Childs ",gs.ObjectArray[0].ToString(),gs.GetChildren.Count);
						if (gs.HasChildren) {
							foreach (SortComparer sc in gs.GetChildren) {
								
								System.Console.WriteLine("\t {0}   {1}",sc.ListIndex,sc.ObjectArray[0].ToString());										}
						}
					} else {
						SortComparer sc = bc as SortComparer;
						
						if (sc != null) {
							System.Console.WriteLine("\t Child {0}",sc.ObjectArray[0].ToString());
						}
					}
					
				}
				*/
			} catch (Exception e) {
				System.Console.WriteLine("BuildGroup {0}",e.Message);
				throw;
			}
		}
		
	
		#endregion
		
		
		#region IEnumerator
		public override bool MoveNext(){
			return base.MoveNext();
		}
		
		public override void Reset() {
			base.Reset();
			this.view.Sort = "";
			this.view.RowFilter = "";
			
		}
		
		public override object Current{
			get {
				if (base.CurrentRow < 0) {
					return null;
				} else {
					return row = this.view[((BaseComparer)base.IndexList[base.CurrentRow]).ListIndex];
				}
			}
		}
		
		#endregion
		
		
		#region IDataViewStrategy interface implementation
		
		public override void Bind() {
			base.Bind();

			if (base.ReportSettings.GroupColumnsCollection.Count > 0) {
				this.Group ();
				Reset();
				return;
			}
			
				this.Sort ();

			Reset();
		}
	
		public override  void Sort () {
			base.Sort();
			if ((base.ReportSettings.SortColumnCollection != null)) {
				if (base.ReportSettings.SortColumnCollection.Count > 0) {
					this.BuildSortIndex (base.IndexList,
					                     base.ReportSettings.SortColumnCollection);

					base.IsSorted = true;
				} else {
					this.BuildPlainIndex(base.IndexList,
					                     base.ReportSettings.SortColumnCollection);
					base.IsSorted = false;
				}
			}
		}
		
		protected override void Group() {
			if (base.ReportSettings.GroupColumnsCollection.Count == 0) {
				return;
			}
			this.BuildGroup();
			base.Group();
		}
		
		public override void Fill (IItemRenderer item) {
			try {
				base.Fill(item);
				if (this.Current == null) {
					System.Console.WriteLine("row is null");
				}
				if (this.row != null) {
					BaseDataItem baseDataItem = item as BaseDataItem;
					if (baseDataItem != null) {
						baseDataItem.DbValue = row[baseDataItem.ColumnName].ToString();
					}
				}
				
			} catch (Exception) {
				throw;
			}
		}
			
	
		public override ColumnCollection AvailableFields {
			get {
				ColumnCollection c = base.AvailableFields;
				DataTable tbl = view.Table;
				for (int i = 0;i < tbl.Columns.Count ;i ++ ) {
					DataColumn col = tbl.Columns[i];
					c.Add (new AbstractColumn(col.ColumnName,col.DataType));
					}
				return c;
			}
		}
		
		
		public override int Count {
			get {
				return this.IndexList.Count;
//				return this.view.Count;
			}
		}
		
		public override int CurrentRow {
			get{
				return base.IndexList.CurrentPosition;
			}
			
			/*
			set {
				base.CurrentRow = value;
				if ((value > -1) && (value < base.IndexList.Count)){
					BaseComparer bc = (BaseComparer)base.IndexList[value];
					
					GroupSeperator sep = bc as GroupSeperator;
					if (sep != null) {
						base.NotifyGroupChanging(this,sep);
					}
					row = this.view[((BaseComparer)base.IndexList[value]).ListIndex];
				}
			}
			*/
		}
		
		#endregion
		
		#region IDisposable
		
		public override  void Dispose(){
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		~TableStrategy(){
			Dispose(false);
		}
		
		protected override void Dispose(bool disposing){
			try {
				if (disposing) {
					if (this.view != null) {
						this.view.Dispose();
						this.view = null;
					}
				}
			} finally {
				// Release unmanaged resources.
				// Set large fields to null.
				// Call Dispose on your base class.
				base.Dispose(disposing);
			}
			
		}
		#endregion
		
	}
}
