using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Timers;
using System.IO;

using Android.Telephony;

namespace GOD_GOD_V1
{
    [Activity(Label = "ATlogin")]
    public class ATlogin : Activity
    {
        Timer tmp = new Timer();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.LYlogin);

            tmp.Interval = 1000;
            tmp.Start();

            TimePicker t= new TimePicker(this);
            DatePicker d = new DatePicker(this);
           int hora = t.CurrentHour.IntValue();
           int minuto = t.CurrentMinute.IntValue();
            int CountAdmin = 0;
           
            string tell="",__IDPROF__="";
            string senha="";
            string hora_inicio="";
            string hora_final = "";
            string turno = "";
            string nome_prof = "";
            string salario_prof = "";
            string curso_prof = "";
            string turma_prof="";
            string id_turma = "",__ANTIGO__,__NOVO__;
            bool atraso = false;
            Dictionary<string, string> DiaActual = new Dictionary<string, string>();
            DiaActual["Monday"] = "segunda-feira";
            DiaActual["Tuesday"] = "terça-feira";
            DiaActual["Wednesday"] = "quarta-feira";
            DiaActual["Thursday"] = "quinta-feira";
            DiaActual["Friday"] = "sexta-feira";

//__________________________________________________MANHÃ______________________________________________________
            if (hora >= 7 && hora <= 12)
            {
                turno = "Manhã";
                if (hora >= 7 && minuto >= 00 && hora <= 7 && minuto < 50)
                {
                    hora_inicio = "07:00";
                    hora_final = "07:50";
                    if (hora == 7 && (minuto > 14 && minuto < 51))
                    {
                        atraso = true;
                    }
                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {

                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    Toast.MakeText(this, salario_prof, ToastLength.Short).Show();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();

                                                    //************************UPDATE ATRASO******************************************************
                                                    if (atraso == true)
                                                    {
                                                        int sal = int.Parse(professor["salario"].ToString());
                                                        __ANTIGO__ = sal.ToString();
                                                       sal -= 800;
                                                        using (var UpdateSalario = new SqliteCommand(connect))
                                                        {
                                                            UpdateSalario.CommandText = "UPDATE professor_tb SET salario=@sal WHERE user_fk=@user";
                                                            UpdateSalario.Parameters.AddWithValue("@sal", sal);
                                                            UpdateSalario.Parameters.AddWithValue("@user", user["id_user"]);
                                                            UpdateSalario.ExecuteNonQuery();
                                                        };
                                                        using (var Actualiza = new SqliteCommand(connect))
                                                        {
                                                            Actualiza.CommandText = "SELECT * FROM professor_tb WHERE user_fk=@user";
                                                            Actualiza.Parameters.AddWithValue("@user",user["id_user"]);
                                                            var getActualiza = Actualiza.ExecuteReader();
                                                            if (getActualiza.HasRows)
                                                            {
                                                                salario_prof = getActualiza["salario"].ToString();
                                                                __NOVO__ = salario_prof;
                                                                string __TEXTO__ = string.Format("Olá professor {0} \n\nHouve um desconto no seu salário por motivos de atraso\n\nSaldo antigo {1}\nSaldo Actual {2}", professor["nome"], __ANTIGO__, __NOVO__);
                                                                SmsManager.Default.SendTextMessage(tell, null, __TEXTO__, null, null);
                                                                Toast.MakeText(this,"Seu salário foi descontado "+salario_prof, ToastLength.Short).Show();
                                                            }
                                                        };

                                                    }
                                                    else
                                                    {
                                                        new AlertDialog.Builder(this)
                                                            .SetMessage("ñ atrasado")
                                                            .Show();
                                                    }
                                                    //************************UPDATE ATRASO******************************************************


                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina",professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables
                                        }


                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a2 = new AlertDialog.Builder(this);
                                    a2.SetMessage("não existe nehum dado");
                                    a2.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________


                }
                else if ((hora >= 7 && minuto >= 55 && hora <= 8) || (hora >= 7 && hora <= 8 && minuto <= 45))
                {
                    hora_inicio = "07:55";
                    hora_final = "08:45";

                    if (hora == 8 && (minuto > 10 && minuto <45))
                    {
                        atraso = true;
                    }

                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                    
                                                }

                                                //************************UPDATE ATRASO******************************************************
                                                if (atraso == true)
                                                {
                                                    int sal = int.Parse(professor["salario"].ToString());
                                                    __ANTIGO__ = sal.ToString();
                                                    sal -= 800;
                                                    using (var UpdateSalario = new SqliteCommand(connect))
                                                    {
                                                        UpdateSalario.CommandText = "UPDATE professor_tb SET salario=@sal WHERE user_fk=@user";
                                                        UpdateSalario.Parameters.AddWithValue("@sal", sal);
                                                        UpdateSalario.Parameters.AddWithValue("@user", user["id_user"]);
                                                        UpdateSalario.ExecuteNonQuery();
                                                    };
                                                    using (var Actualiza = new SqliteCommand(connect))
                                                    {
                                                        Actualiza.CommandText = "SELECT * FROM professor_tb WHERE user_fk=@user";
                                                        Actualiza.Parameters.AddWithValue("@user", user["id_user"]);
                                                        var getActualiza = Actualiza.ExecuteReader();
                                                        if (getActualiza.HasRows)
                                                        {
                                                            salario_prof = getActualiza["salario"].ToString();
                                                            __NOVO__ = salario_prof;
                                                            string __TEXTO__ = string.Format("Olá professor {0} \n\nHouve um desconto no seu salário por motivos de atraso\n\nSaldo antigo {1}\nSaldo Actual {2}", professor["nome"], __ANTIGO__, __NOVO__);
                                                            SmsManager.Default.SendTextMessage(tell, null, __TEXTO__, null, null);
                                                            Toast.MakeText(this, "Seu salário foi descontado " + salario_prof, ToastLength.Short).Show();
                                                        }
                                                    };

                                                }
                                                else
                                                {
                                                    new AlertDialog.Builder(this)
                                                        .SetMessage("ñ atrasado")
                                                        .Show();
                                                }
                                                //************************UPDATE ATRASO******************************************************



                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables
                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________


                }



                else if ((hora >= 8 && minuto <= 50 && hora <= 9) || (hora >= 8 && hora <= 9 && minuto <= 40))
                {
                    hora_inicio = "08:50";
                    hora_final = "09:40";
                   
                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia = @dia ";
                                comand.Parameters.AddWithValue("@hin",hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Professor ( " + nome_prof + ")\n");
                                                    alerta.Show();
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables


                                        }
                                        using (var cmd_turma = new SqliteCommand(connect))
                                        {
                                            cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                            cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                            var turma_fk = cmd_turma.ExecuteReader();
                                            if (turma_fk.HasRows)
                                            {
                                                id_turma = turma_fk["id_turma"].ToString();
                                                turma_prof = turma_fk["turma"].ToString();
                                                using (var cmd_curso = new SqliteCommand(connect))
                                                {
                                                    cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                    cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                    var curso = cmd_curso.ExecuteReader();
                                                    if (curso.HasRows)
                                                    {
                                                        curso_prof = curso["curso"].ToString();

                                                    }

                                                }
                                            }

                                        }

                                    };///professor tabela
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________


                }


                else if (hora >= 10 && minuto >= 00 && hora <= 10 && minuto <= 50)
                {
                    var a2 = new AlertDialog.Builder(this);
                   
                    hora_inicio = "10:00";
                    hora_final = "10:50";
                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables

                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________
                }


                else if ( (hora >= 10 && minuto <= 55 && hora <= 11) || (hora >= 10 && hora <= 11 && minuto <= 45) )
                {
                   
                    hora_inicio = "10:55";
                    hora_final = "11:45";

                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                   
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables

                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________

                }


                else if ((hora >= 11 && minuto <= 50 && hora <= 12) || (hora >= 11 && hora <= 12 && minuto <= 40))
                {
                    
                    hora_inicio = "11:50";
                    hora_final = "12:40";


                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                    
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables
                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________



                }




            }//end turno da manhã



            //__________________________________________________TARDE______________________________________________________

            
            else if (hora >= 13 && hora <= 18)
            {
                turno = "Tarde";

                if (hora >= 13 && minuto >= 00 && hora <= 13 && minuto <= 50)
                {
                    hora_inicio = "13:00";
                    hora_final = "13:50";


                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;

                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables
                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________


                }


                else if ((hora >= 13 && minuto >= 55 && hora <= 14) || (hora >= 13 && hora <= 14 && minuto <= 45))
                {
                    hora_inicio = "13:55";
                    hora_final = "14:45";
                    
                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables
                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________


                }


                else if ((hora >= 14 && minuto >= 50 && hora<=15) || (hora >= 14 && hora <= 15 && minuto <= 40))
                {
                    hora_inicio = "14:50";
                    hora_final = "15:40";


                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables

                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________


                }


                else if ((hora >= 15 && minuto >= 45 && hora <= 16) || (hora >= 15 && hora <= 16 && minuto <= 35) || (hora>=15 && minuto>44) )
                {
                   
                    hora_inicio = "15:45";
                    hora_final = "16:35";
                    
                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables

                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________

                }


                else if ((hora >= 16 && minuto >= 40 && hora <= 17) || (hora >= 16 && hora <= 17 && minuto <= 30))
                {
                  
                    hora_inicio = "16:40";
                    hora_final = "17:30";

                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables
                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________


                }

                else if ((hora >= 17 && minuto >= 35 && hora <= 18) || (hora >= 17 && hora <= 18 && minuto <= 25))
                {//close point book
                    
                    hora_inicio = "17:35";
                    hora_final = "18:25";
                    //__________________________________________GetDATA____________________________________________________________________
                    try
                    {
                        using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();
                            using (var comand = new SqliteCommand(connect))
                            {
                                comand.CommandText = "SELECT * FROM horario_tb WHERE hora_inicio=@hin and hora_fim=@hfn and periodo=@p and dia =@dia";
                                comand.Parameters.AddWithValue("@hin", hora_inicio);
                                comand.Parameters.AddWithValue("@hfn", hora_final);
                                comand.Parameters.AddWithValue("@p", turno);
                                comand.Parameters.AddWithValue("@dia", DiaActual[d.DateTime.DayOfWeek.ToString()]);
                                var horario = comand.ExecuteReader();

                                if (horario.HasRows)
                                {
                                    //definir a turma 
                                    using (var cmd_professor = new SqliteCommand(connect))
                                    {
                                        cmd_professor.CommandText = "SELECT * FROM professor_tb WHERE disciplina_fk=@disciplina";
                                        cmd_professor.Parameters.AddWithValue("@disciplina", horario["disciplina_fk"]);
                                        var professor = cmd_professor.ExecuteReader();

                                        if (professor.HasRows)
                                        {
                                           
                                            using (var cmd_user = new SqliteCommand(connect))
                                            {
                                                cmd_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                                cmd_user.Parameters.AddWithValue("@user", professor["user_fk"]);
                                                var user = cmd_user.ExecuteReader();
                                                if (user.HasRows)
                                                {
                                                    CountAdmin = (user["tipo_conta"].ToString() == "admin") ? 1 : 0;
                                                    nome_prof = professor["nome"].ToString();
                                                    salario_prof = professor["salario"].ToString();
                                                    __IDPROF__ = user["id_user"].ToString();
                                                    tell = user["telefone"].ToString();
                                                    senha = user["senha"].ToString();
                                                }

                                            };//user tabela

                                            using (var cmd_disciplina = new SqliteCommand(connect))
                                            {
                                                cmd_disciplina.CommandText = "SELECT * FROM disciplina_tb WHERE id_disciplina=@disciplina";
                                                cmd_disciplina.Parameters.AddWithValue("@disciplina", professor["disciplina_fk"].ToString());
                                                var disciplina_tb = cmd_disciplina.ExecuteReader();
                                                if (disciplina_tb.HasRows)
                                                {
                                                    var alerta = new AlertDialog.Builder(this);
                                                    alerta.SetMessage("Tempo Reservado Ao Professor \"" + professor["nome"] + "\"\n\n" +
                                                        "da disciplina :  \"" + disciplina_tb["disciplina"] + "\" ");
                                                    alerta.Show();
                                                }

                                            };//disciplina tables


                                        }

                                    };///professor tabela
                                    using (var cmd_turma = new SqliteCommand(connect))
                                    {
                                        cmd_turma.CommandText = "SELECT * FROM turma_tb WHERE id_turma=@turma";
                                        cmd_turma.Parameters.AddWithValue("@turma", horario["turma_fk"]);
                                        var turma_fk = cmd_turma.ExecuteReader();
                                        if (turma_fk.HasRows)
                                        {
                                            id_turma = turma_fk["id_turma"].ToString();
                                            turma_prof = turma_fk["turma"].ToString();
                                            using (var cmd_curso = new SqliteCommand(connect))
                                            {
                                                cmd_curso.CommandText = "SELECT * FROM curso_tb WHERE id_curso=@curso";
                                                cmd_curso.Parameters.AddWithValue("@curso", turma_fk["curso_fk"]);
                                                var curso = cmd_curso.ExecuteReader();
                                                if (curso.HasRows)
                                                {
                                                    curso_prof = curso["curso"].ToString();

                                                }

                                            }
                                        }

                                    }
                                }//horario
                                else
                                {
                                    var a22 = new AlertDialog.Builder(this);
                                    a22.SetMessage("não existe nehum dado");
                                    a22.Show();
                                }
                            };
                            connect.Close();
                            connect.Dispose();
                        };
                    }
                    catch (System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Long).Show();
                    }

                    //______________________________________________ENDGETDATA_________________________________________________________________
                    
                }

                //end turno da tarde
            }//end turno da tarde



            var btn_entrar = FindViewById<Button>(Resource.Id.loginBtnEntrar);
            var btn_recuperara = FindViewById<Button>(Resource.Id.loginBtnRecuperar);
            var Username = FindViewById<TextView>(Resource.Id.loginNome);
            Username.InputType = Android.Text.InputTypes.ClassPhone;
            var password = FindViewById<TextView>(Resource.Id.loginSenha);
            bool admin = false;

            bool logado = false;

            btn_entrar.Click += (sender, evento) => 
            {
                
                if (Username.Text == "" || password.Text == "")
                {
                    var alert = new AlertDialog.Builder(this)
                        .SetIcon(Resource.Drawable.warning)
                        .SetMessage("Favor preencher todos os campo")
                        .SetTitle("Aviso");
                    alert.Show();

                }
                else if (Username.Text.Length<9 || password.Text.Length<3)
                {
                    var alert = new AlertDialog.Builder(this)
                       .SetIcon(Resource.Drawable.warning)
                       .SetMessage("Verifique seu dados")
                       .SetTitle("Aviso");
                    alert.Show();

                }
                else if (Username.Text== tell && password.Text==senha )
                {
                    logado = true;
                    var AThome = new Intent(this, typeof(AT_home));
                    AThome.PutExtra("tipo_conta","prof");
                    AThome.PutExtra("nome_prof",nome_prof);
                    AThome.PutExtra("salario_prof",salario_prof);
                    AThome.PutExtra("curso_prof",curso_prof);
                    AThome.PutExtra("turma_prof",turma_prof);
                    AThome.PutExtra("id_turma",id_turma);
                    AThome.PutExtra("hora_final",hora_final);
                    AThome.PutExtra("hora_inicio",hora_inicio);
                    AThome.PutExtra("caminho",Intent.GetStringExtra("caminho"));
                    AThome.PutExtra("__id__",__IDPROF__);
                    
                    View lyBox = LayoutInflater.FromContext(this).Inflate
                    (Resource.Layout.LYbox, null, false);
                    var alert = new AlertDialog.Builder(this)
                     .SetView(lyBox);
                    alert.Show();
                    var btn2Box = lyBox.FindViewById<Button>(Resource.Id.button2);
                    btn2Box.Click += (em, en) =>
                    {
                        
                        StartActivity(AThome);
                        this.Finish();
                    };
                    
                }
                
                if (Username.Text !="" && password.Text !="" && CountAdmin==0)
                {
                    admin = true;
                    int id_curso = 0,turma_fk=0,id_user=0;

                    using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")) )
                    {
                        connect.Open();
                        try
                        {
                            using (var cmd = new SqliteCommand(connect))
                            {
                                cmd.CommandText = "SELECT * FROM user_tb where telefone=@tell and senha=@senha and tipo_conta=@tipo;";
                                cmd.Parameters.AddWithValue("@tell", Username.Text);
                                cmd.Parameters.AddWithValue("@senha", password.Text);
                                cmd.Parameters.AddWithValue("@tipo", "admin");
                                var admin_tb = cmd.ExecuteReader();

                                if (admin_tb.HasRows)
                                {
                                    var AThome = new Intent(this, typeof(AT_home));
                                    AThome.PutExtra("tipo_conta", "admin");AThome.PutExtra("caminho",Intent.GetStringExtra("caminho"));
                                    //getTurma ,Get Curso

                                    using (var GetDireitor=new SqliteCommand(connect))
                                    {
                                        GetDireitor.CommandText = "SELECT * FROM  direitor_tb WHERE  user_fk=@direitor";
                                        GetDireitor.Parameters.AddWithValue("@direitor",admin_tb["id_user"]);
                                        var direitor_tb = GetDireitor.ExecuteReader();
                                        if (direitor_tb.HasRows)
                                        {
                                            AThome.PutExtra("nome_admin",direitor_tb["nome"].ToString());
                                            AThome.PutExtra("id_direitor", direitor_tb["id_direitor"].ToString());

                                            using (var getCourse = new SqliteCommand(connect))
                                            {
                                                getCourse.CommandText = "SELECT * FROM curso_tb WHERE direitor_fk=@dir_fk";
                                                getCourse.Parameters.AddWithValue("@dir_fk",direitor_tb["id_direitor"]);
                                                var curso_tb = getCourse.ExecuteReader();
                                                if (curso_tb.HasRows)
                                                {
                                                    AThome.PutExtra("curso_admin", curso_tb["curso"].ToString());
                                                    using (var getTurma= new SqliteCommand(connect))
                                                    {
                                                        getTurma.CommandText = "SELECT * FROM turma_tb WHERE curso_fk=@curso_fk";
                                                        getTurma.Parameters.AddWithValue("@curso_fk",curso_tb["id_curso"]);
                                                        var turma_tb = getTurma.ExecuteReader();

                                                        if (turma_tb.HasRows)
                                                        {
                                                            AThome.PutExtra("turma_admin",turma_tb["turma"].ToString());
                                                        }

                                                    }
                                                }

                                            };
                                        }
                                    };


                                    View lyBox = LayoutInflater.FromContext(this).Inflate
                                    (Resource.Layout.LYbox, null, false);
                                    var alert = new AlertDialog.Builder(this)
                                    .SetView(lyBox);
                                    alert.Show();
                                    var btn2Box = lyBox.FindViewById<Button>(Resource.Id.button2);
                                    btn2Box.Click += (em, en) =>
                                    {
                                        
                                        StartActivity(AThome);
                                        this.Finish();
                                    };
                                }
                                else
                                {
                                    if (CountAdmin == 0)
                                    {
                                        var alert = new AlertDialog.Builder(this)
                                       .SetMessage("Verifique sua senha de usuário e telefone ");
                                        alert.Show();
                                    }
                                  

                                }
                            }
                        }
                        catch(System.Exception erro)
                        {
                            Toast.MakeText(this,erro.Message, ToastLength.Short);
                        }

                        connect.Close();
                        connect.Dispose();
                    };

 
                }


                else if (Username.Text != tell && password.Text != senha && admin == false)
                {
                    var alert = new AlertDialog.Builder(this)
                    .SetMessage("Não esta permitdo para acessar o livro \neste tempo esta reservado para outro professor\n\nObrigada pela conpreenção");
                    alert.Show();
                }
                //btn .click
            };




            //*********************************************mark fauls on teacher************************************************
            string[] convertTime = hora_final.Split(':');
           
            tmp.Elapsed += (ty, ti) => {

                RunOnUiThread(() => {
                    try
                    {
                        if (logado == false)
                        {
                            //verificar se o tempo final ja chegou 
                            if(hora==int.Parse(convertTime[0].ToString()) &&  (  minuto>= int.Parse(convertTime[1].ToString())  && minuto<= int.Parse(convertTime[1])+3 )  )
                            {
                                Toast.MakeText(this, convertTime[0] + " (Falta) " + convertTime[1], ToastLength.Short).Show();
                            } 
                           
                        }
                        else
                        {
                            //Toast.MakeText(this, convertTime[0] + "work on" + convertTime[1], ToastLength.Short).Show();
                        }
                       
                    }
                    catch (System.Exception erro)
                    {
                        tmp.Stop();
                    }



                });
            };

            
            //*********************************************mark fauls on teacher************************************************




            //____________________________________RECUPERARA SENHA__________________________________________________
            btn_recuperara.Click += (sender, evento) =>
            {
                View Recoverlayout = LayoutInflater.FromContext(this).Inflate(
                        Resource.Layout.LYmensagem, null, false);
                
                var alert = new AlertDialog.Builder(this)
                .SetView(Recoverlayout);
                alert.Create();
                alert.Show();

                var msgBtn = Recoverlayout.FindViewById<Button>(Resource.Id.msgBtn);
                var phoneOrigem = Recoverlayout.FindViewById<EditText>(Resource.Id.msgTelefone);
                var phoneDestino = Recoverlayout.FindViewById<EditText>(Resource.Id.msgAdminPhone);
                var Sms = Recoverlayout.FindViewById<EditText>(Resource.Id.msgDescricao);
                msgBtn.Click += (even, send) =>
                {
                    if ((phoneOrigem.Text == "" || phoneDestino.Text == ""))
                    {
                        
                        var alerta = new AlertDialog.Builder(this)
                        .SetMessage("favor preencher todos os campos");
                        alerta.Create();
                        alerta.Show();
                    }
                    else if (phoneOrigem.Text != "" && phoneDestino.Text != "" && Sms.Text != "")
                    {
                        int origemId = 0, destinoId = 0;
                        int MaxId = 0;

                        if (phoneOrigem.Text != phoneDestino.Text && phoneDestino.Text != phoneOrigem.Text)
                        {
                            using (var connect = new SqliteConnection("Data source = " + Intent.GetStringExtra("caminho")))
                            {
                                connect.Open();
                                //________________________________________________________________________________________________
                                using (var user_tbOrigem = new SqliteCommand(connect))
                                {
                                    user_tbOrigem.CommandText = "SELECT * FROM user_tb WHERE  telefone=@tell";
                                    user_tbOrigem.Parameters.AddWithValue("@tell", phoneOrigem.Text);
                                    var origem = user_tbOrigem.ExecuteReader();
                                    if (origem.HasRows)
                                    {
                                        origemId = int.Parse(origem["id_user"].ToString());
                                        //var alerta = new AlertDialog.Builder(this)
                                        //.SetMessage("origem =>" + origem["id_user"].ToString());
                                        //alerta.Create();
                                        //alerta.Show();
                                    }
                                }
                                //________________________________________________________________________________________________


                                //________________________________________________________________________________________________
                                using (var user_tbDestino = new SqliteCommand(connect))
                                {
                                    user_tbDestino.CommandText = "SELECT * FROM user_tb WHERE  telefone=@tell and tipo_conta=@c";
                                    user_tbDestino.Parameters.AddWithValue("@tell", phoneDestino.Text);
                                    user_tbDestino.Parameters.AddWithValue("@c", "admin");
                                    var destino = user_tbDestino.ExecuteReader();
                                    if (destino.HasRows)
                                    {
                                        destinoId = int.Parse(destino["id_user"].ToString());

                                        
                                    }
                                }

                                //________________________________________________________________________________________________
                                //________________________________________________________________________________
                                using (var InsertMessage = new SqliteCommand(connect))
                                {
                                    InsertMessage.CommandText = "select max(id_sms) as maxId from mensagem_tb;";
                                    var GetMaxId = InsertMessage.ExecuteReader();
                                    if (GetMaxId.HasRows)
                                    {
                                        if (GetMaxId.HasRows)
                                        {
                                            if (GetMaxId["maxId"].ToString() == "")
                                            {
                                                MaxId = 0 + 1;
                                            //    var alerta = new AlertDialog.Builder(this)
                                            //.SetMessage(MaxId+" => caso0");
                                            //    alerta.Create();
                                            //    alerta.Show();
                                            }
                                            else if (GetMaxId["maxId"].ToString() != "")
                                            {
                                                MaxId = int.Parse(GetMaxId["maxId"].ToString()) + 1;
                                           //     var alerta = new AlertDialog.Builder(this)
                                           //.SetMessage(MaxId + " => caso1\n(" + GetMaxId["maxId"].ToString() + ")");
                                           //     alerta.Create();
                                           //     alerta.Show();
                                            }
                                            try
                                            {
                                                using (var InserirSMS = new SqliteCommand(connect))
                                                {
                                                    InserirSMS.CommandText = "INSERT INTO mensagem_tb (id_sms,origem,destino,texto) " +
                                                    "VALUES (@id_sms,@origem,@destino,@texto)";
                                                    InserirSMS.Parameters.AddWithValue("@id_sms", MaxId);
                                                    InserirSMS.Parameters.AddWithValue("@origem", origemId);
                                                    InserirSMS.Parameters.AddWithValue("@destino", destinoId);
                                                    InserirSMS.Parameters.AddWithValue("@texto", Sms.Text);
                                                    InserirSMS.ExecuteNonQuery();
                                                    Toast.MakeText(this, "Mensagem Enviada para  " + phoneDestino.Text, ToastLength.Long).Show();
                                                  
                                                };
                                            }
                                            catch (System.Exception error)
                                            {
                                                Toast.MakeText(this, error.Message, ToastLength.Long).Show();
                                            }

                                        }
                                    }


                                }
                                //________________________________________________________________________________________________


                                connect.Close();
                                connect.Dispose();
                            };
                        }
                        else if(phoneOrigem.Text==phoneDestino.Text && phoneDestino.Text==phoneOrigem.Text)
                        {
                            var alerta = new AlertDialog.Builder(this)
                            .SetMessage("Seu número não deve ser o mesmo que o do administrador");
                            alerta.Create();
                            alerta.Show();
                        }
                    }
                };
            };
            //_______________________________________________RECUPERARA SENHA__________________________________________________________________

           
           
        }
    }
}