using System;
using System.Collections.Generic;

using Android.App;
using Android.Widget;
using Android.OS;

using SQLite;
using System.Linq;
using System.Collections;

namespace banco
{
    [Activity(Label = "banco", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            SetContentView(Resource.Layout.Main);


            var btnCreate = FindViewById<Button>(Resource.Id.btnCreateDB);
            var btnSingle = FindViewById<Button>(Resource.Id.btnCreateSingle);
            var btnList = FindViewById<Button>(Resource.Id.btnList);
            var btnShow = FindViewById<Button>(Resource.Id.btnShow);
            var btnClear = FindViewById<Button>(Resource.Id.btnClear);
            var txtResult = FindViewById<TextView>(Resource.Id.txtResults);


            // caminho do BD
            var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db");

            // Desabilita os botões enquanto não tem o BD criado
            btnSingle.Enabled = btnList.Enabled = false;



            btnCreate.Click += delegate
            {
                var result = createDatabase(pathToDatabase);
                txtResult.Text = result + "\n";
                if (result == "Banco de dados criado")
                    btnList.Enabled = btnSingle.Enabled = true;
            };




            btnSingle.Click += delegate
            {
                var result = insertUpdateData(new Person { FirstName = "João", LastName = "Silva" }, pathToDatabase);
                var records = findNumberRecords(pathToDatabase);
                txtResult.Text += string.Format("{0}\nNumero de registros = {1}\n", result, records);
            };

         

            btnList.Click += delegate
            {
                var peopleList = new List<Person>
                {
                    new Person { FirstName = "Miguel", LastName = "Santos" },
                    new Person { FirstName = "Carlos", LastName = "Barros" },
                    new Person { FirstName = "Maria", LastName = "Silvana" }
                };
                var result = insertUpdateAllData(peopleList, pathToDatabase);
                var records = findNumberRecords(pathToDatabase);
                txtResult.Text += string.Format("{0}\nNumero de registros = {1}\n", result, records);
            };
        }

        private string createDatabase(string path)
        {
            try
            {
                var connection = new SQLiteConnection(path);
                connection.CreateTable<Person>();
                return "Banco de dados criado";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

      


        private string insertUpdateData(Person data, string path)
        {
            try
            {
                var db = new SQLiteConnection(path);
                if (db.Insert(data) != 0)
                    db.Update(data);
                return "Registro unico inserido ou atualizado";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        private string insertUpdateAllData(IEnumerable<Person> data, string path)
        {
            try
            {
                var db = new SQLiteConnection(path);
                if (db.InsertAll(data) != 0)
                    db.UpdateAll(data);
                return "Lista de registros inseridos ou atualizados";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        private int findNumberRecords(string path)
        {
            try
            {
                var db = new SQLiteConnection(path);
                var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person");
                return count;
            }
            catch (SQLiteException)
            {
                return -1;
            }
        }

    
    }
}