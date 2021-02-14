using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;

using Android.Telephony;

namespace GOD_GOD_V1
{
    class AdminSMS : BaseAdapter<string>
    {
        private Context mContext;
        private List<string> mList;
        private List<string> mTell;
        private List<string> mTexto;
        string DataBase;
        public string Data(string DB)
        {
            DataBase = DB;
            return DB.ToString();
        }

        public AdminSMS (Context context,List<string>names, List<string> tell, List<string> texto)
        {
            mContext = context;
            mList = names;
            mTell = tell;
            mTexto = texto;
        }

        public override int Count
        {
            get {return  mTell.Count; }
        }

        public override string this[int position]
        {
            get { return mTell[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row;
            row = convertView;
            if (row == null)
            {
                row = LayoutInflater.FromContext(mContext).Inflate
                    (Resource.Layout.AdminListItemSMS,null,false);
            }

            var __nomes__ = row.FindViewById<TextView>(Resource.Id.__NOME__ADMIN__);
            __nomes__.Text = mList[position];

            var __telle__ = row.FindViewById<TextView>(Resource.Id.__TELL__ADMIN__);
            __telle__.Text = mTell[position];

            var __txt__ = row.FindViewById<TextView>(Resource.Id.__TEXTO__ADMIN);
            __txt__.Text = mTexto[position];

            var __btn__= row.FindViewById<Button>(Resource.Id.__BTN__ADMIN);
            __btn__.Click += (la, le) =>
            {
                try
                {
                    using (var con = new SqliteConnection("Data source=" + DataBase))
                    {
                        con.Open();
                        using (var cmd= new SqliteCommand(con))
                        {
                            cmd.CommandText = "SELECT * FROM user_tb WHERE telefone=@tell";
                            cmd.Parameters.AddWithValue("@tell", mTell[position].ToString());
                            var senha = cmd.ExecuteReader();
                            if (senha.HasRows)
                            {
                                try
                                {
                                    
                                    SmsManager.Default.SendTextMessage(mTell[position], null,"Olá  senhor "+ mList[position] +"\nSua senha foi recuperada :"+senha["senha"].ToString(), null, null);
                                    Toast.MakeText(mContext, "Solicitão de senha enviada", ToastLength.Short).Show();
                                }
                                catch(System.Exception erro)
                                {
                                    Toast.MakeText(mContext, "Solicitão de senha não enviada", ToastLength.Short).Show();
                                }
                            }
                        }
                        con.Close();
                        con.Dispose();
                    };
                    

                    
                }catch(System.Exception erro)
                {
                    Toast.MakeText(mContext,erro.Message, ToastLength.Short).Show();
                }
            };


            return row;
        }
    }
}