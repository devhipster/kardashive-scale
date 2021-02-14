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

namespace GOD_GOD_V1
{
    class ProfList : BaseAdapter<string>
    {
        private Context mContext;
        private List<string> mList,mCode,mTell,mFalta;
        private SqliteConnection Data;
        private string hin, hfn, datta;
        public ProfList(Context context,List<string>names, List<string> code , List<string> tell)
        {
            mContext = context;
            mList = names;
            mCode = code;
            mTell = tell;
           
        }
        public void SetElements(string data,string timerIn, string timerFn)
        {
            hin = timerIn;
            hfn = timerFn;
            datta = data;

        }

        public SqliteConnection GetDataBase(SqliteConnection data)
        {
            Data = data;
            return data;
        }

        public override int Count
        {
            get {return  mCode.Count; }
        }

        public override string this[int position]
        {
            get { return mCode[position]; }
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
                    (Resource.Layout.ListAlunosProf,null,false);
            }
            //*****************************definição das lista*****************************************
            Switch escolha = row.FindViewById<Switch>(Resource.Id.switch1);
            TextView saida = row.FindViewById<TextView>(Resource.Id.textSaida);
            saida.Text = "Falta: 3F | Código: "+mCode[position]+" | Tell: "+mTell[position]+"";
            escolha.Text = mList[position];
            //****************************definição das lista*****************************************

            escolha.Click += (we,wa) =>
            {
                if (escolha.Checked == true)
                {
                   //**************************************_____MARCAR FALTA PARA PROFESSOR_____*****************************************
                    try
                    {
                       
                        Data.Open();
                        //__PRESENCA__PROF.CommandText = "UPDATE  presenca_aluno SET falta =@fl WHERE aluno_fk=@al and hora_fim=@hfn and hora_inicio=@hin and data =@da";

                        var __PRESENCA__PROF = new SqliteCommand(Data);
                        __PRESENCA__PROF.CommandText = "UPDATE  presenca_aluno SET falta =@fl WHERE aluno_fk=@al and data =@da and hora_fim=@hfn and hora_inicio=@hin";
                        __PRESENCA__PROF.Parameters.AddWithValue("@fl","Sim");
                        __PRESENCA__PROF.Parameters.AddWithValue("@al",mCode[position]);
                        __PRESENCA__PROF.Parameters.AddWithValue("@da", datta);
                        __PRESENCA__PROF.Parameters.AddWithValue("@hfn",hfn);
                        __PRESENCA__PROF.Parameters.AddWithValue("@hin",hin);
                        
                        __PRESENCA__PROF.ExecuteNonQuery();

                        using (var __SELECTFALTA_= new SqliteCommand(Data))
                        {
                            __SELECTFALTA_.CommandText = "SELECT * FROM presenca_aluno WHERE aluno_fk=@al";
                            __SELECTFALTA_.Parameters.AddWithValue("@al", mCode[position]);
                            var __readerAluno__ = __SELECTFALTA_.ExecuteReader();
                            if (__readerAluno__.HasRows)
                            {
                                if (__readerAluno__["falta"].ToString() == "Sim")
                                {
                                Toast.MakeText(mContext, mList[position] + " presente ", ToastLength.Short).Show();

                                }
                               // Toast.MakeText(mContext, mList[position] + "presença == "+__readerAluno__["falta"].ToString(), ToastLength.Short).Show();
                            }
                        };

                        using (var __SELECTcount_ = new SqliteCommand(Data))
                        {
                            __SELECTcount_.CommandText = "SELECT count(*) as fl ,falta FROM presenca_aluno WHERE aluno_fk=@al and falta=@fal ";
                            __SELECTcount_.Parameters.AddWithValue("@al", mCode[position]);
                            __SELECTcount_.Parameters.AddWithValue("@fal", "Sim");
                            var __readerCount__ = __SELECTcount_.ExecuteReader();

                            if (__readerCount__.HasRows)
                            {
                                new AlertDialog.Builder(mContext)
                                .SetMessage("nº de faltas do " + mList[position] + " (" + __readerCount__["fl"].ToString() + ")"
                                + " (" + __readerCount__["falta"].ToString() + ")")
                                .Show();

                                // Toast.MakeText(mContext, mList[position] + "presença == "+__readerAluno__["falta"].ToString(), ToastLength.Short).Show();
                            }
                        };

                        Data.Close();
                        Data.Dispose();

                       
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(mContext,erro.Message, ToastLength.Short).Show();
                    }
                    //**************************************_____MARCAR FALTA PARA PROFESSOR_____*****************************************

                }//marcando presença escolha ==true


                else if(escolha.Checked==false)
                {

                    //**************************************_____MARCAR FALTA PARA PROFESSOR_____*****************************************
                    try
                    {
                        //"@hora_inicio"
                        //"@hora_fim", I
                        //"@data", new D

                        Data.Open();

                        var __PRESENCA__PROF = new SqliteCommand(Data);
                        __PRESENCA__PROF.CommandText = "UPDATE  presenca_aluno SET falta =@fl WHERE aluno_fk=@al and data =@da and hora_fim=@hfn and hora_inicio=@hin";
                        __PRESENCA__PROF.Parameters.AddWithValue("@fl", "Não");
                        __PRESENCA__PROF.Parameters.AddWithValue("@al", mCode[position]);
                        __PRESENCA__PROF.Parameters.AddWithValue("@da", datta);
                        __PRESENCA__PROF.Parameters.AddWithValue("@hfn", hfn);
                        __PRESENCA__PROF.Parameters.AddWithValue("@hin", hin);

                        __PRESENCA__PROF.ExecuteNonQuery();

                        using (var __SELECTFALTA_ = new SqliteCommand(Data))
                        {
                            __SELECTFALTA_.CommandText = "SELECT * FROM presenca_aluno WHERE aluno_fk=@al";
                            __SELECTFALTA_.Parameters.AddWithValue("@al", mCode[position]);
                            var __readerAluno__ = __SELECTFALTA_.ExecuteReader();
                            if (__readerAluno__.HasRows)
                            {
                                if (__readerAluno__["falta"].ToString() == "Não")
                                {

                                    Toast.MakeText(mContext, mList[position] + " esta ausente ", ToastLength.Short).Show();

                                }
                               // Toast.MakeText(mContext, mList[position] + " presença == " + __readerAluno__["falta"].ToString(), ToastLength.Short).Show();
                            }
                        };

                        using (var __SELECTcount_ = new SqliteCommand(Data))
                        {
                            __SELECTcount_.CommandText = "SELECT count(*) as fl ,falta FROM presenca_aluno WHERE aluno_fk=@al and falta=@fal ";
                            __SELECTcount_.Parameters.AddWithValue("@al", mCode[position]);
                            __SELECTcount_.Parameters.AddWithValue("@fal", "Não");
                            var __readerCount__ = __SELECTcount_.ExecuteReader();

                            if (__readerCount__.HasRows)
                            {
                                new AlertDialog.Builder(mContext)
                                .SetMessage("nº de faltas do " + mList[position] + " (" + __readerCount__["fl"].ToString() + ")"
                                + " (" + __readerCount__["falta"].ToString() + ")")
                                .Show();

                                // Toast.MakeText(mContext, mList[position] + "presença == "+__readerAluno__["falta"].ToString(), ToastLength.Short).Show();
                            }
                        };

                        Data.Close();
                        Data.Dispose();


                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(mContext, erro.Message, ToastLength.Short).Show();
                    }
                    //**************************************_____MARCAR FALTA PARA PROFESSOR_____*****************************************

                }//marcando Ausencia escolha ==false

            };//terminado o evento click do switch

            return row;
        }
    }
}