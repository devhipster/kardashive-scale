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
using System.Timers;
using Android.Widget;

namespace GOD_GOD_V1
{
    [Activity(Label = "AT_home")]
    public class AT_home : Activity
    {
        List<string> mItems, mcodes, mtells,mfalta;
        Timer tmp = new Timer();

        public string getData()
        {
            return "";
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            string conta = Intent.GetStringExtra("tipo_conta");
            if (conta == "admin")
            {

                SetContentView(Resource.Layout.LYadmin_home);

                var AdminNome = FindViewById<TextView>(Resource.Id.AdminNome);
                AdminNome.Text = Intent.GetStringExtra("nome_admin");
                var AdminCurso = FindViewById<TextView>(Resource.Id.AdminCurso);
                AdminCurso.Text = Intent.GetStringExtra("curso_admin");
                var AdminTurma = FindViewById<TextView>(Resource.Id.AdminTurma);
                AdminTurma.Text = Intent.GetStringExtra("turma_admin");



                //____________________________opção1_____________________________________

                var imgSMS = FindViewById<ImageView>(Resource.Id.imgSms);
                var textSMS = FindViewById<TextView>(Resource.Id.textSms);

                //************************************getMessage Data**********************************

                var listaNomes = new List<string>();
                var listaTelefone = new List<string>();
                var listaTexto = new List<string>();

                using (var connect = new SqliteConnection("Data source=" + Intent.GetStringExtra("caminho")))
                {

                    connect.Open();
                    using (var dirSelect = new SqliteCommand(connect))
                    {


                        dirSelect.CommandText = "SELECT * FROM mensagem_tb WHERE destino=@destino";
                        dirSelect.Parameters.AddWithValue("@destino", Intent.GetStringExtra("id_direitor"));
                        var dir_tb = dirSelect.ExecuteReader();
                        while (dir_tb.Read())
                        {
                            var cmd_prof = new SqliteCommand(connect);
                            cmd_prof.CommandText = "SELECT * FROM professor_tb WHERE user_fk=@user";

                            cmd_prof.Parameters.AddWithValue("@user", dir_tb["origem"]);
                            var reader = cmd_prof.ExecuteReader();

                            // new AlertDialog.Builder(this)
                            //.SetMessage(dir_tb["origem"].ToString()+" nome => "+reader["nome"])
                            //.Show();
                            listaNomes.Add(reader["nome"].ToString());

                            listaTexto.Add(dir_tb["texto"].ToString());
                            var cmd_tel_user = new SqliteCommand(connect);
                            cmd_tel_user.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                            cmd_tel_user.Parameters.AddWithValue("@user", dir_tb["origem"]);
                            var readerUser = cmd_tel_user.ExecuteReader();

                            listaTelefone.Add(readerUser["telefone"].ToString());
                        }
                    }

                    connect.Close();
                    connect.Dispose();
                }
                AdminSMS smsList = new AdminSMS(this, listaNomes, listaTelefone, listaTexto);
                smsList.Data(Intent.GetStringExtra("caminho"));
                //************************************getMessage Data**********************************


                imgSMS.Click += (e, s) => {
                    View SMSlayout = LayoutInflater.FromContext(this).Inflate(
                    Resource.Layout.LYadminMessage, null, false);
                    var ListSms = SMSlayout.FindViewById<ListView>(Resource.Id.listaSms);
                    //ProfList professor = new ProfList(this, listaNomes);
                    //ArrayAdapter ad = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);

                    ListSms.FastScrollEnabled = true;
                    ListSms.Adapter = smsList;
                    var alert = new AlertDialog.Builder(this)
                        .SetView(SMSlayout);
                    alert.Show();
                };


                textSMS.Click += (e, s) => {

                    View SMSlayout = LayoutInflater.FromContext(this).Inflate(
                    Resource.Layout.LYadminMessage, null, false);
                    var ListSms = SMSlayout.FindViewById<ListView>(Resource.Id.listaSms);

                    // AdminSMS smsList = new AdminSMS(this, listaNomes,listaTelefone, listaTexto);

                    ListSms.FastScrollEnabled = true;
                    ListSms.Adapter = smsList;
                    var alert = new AlertDialog.Builder(this)
                    .SetView(SMSlayout);
                    alert.Show();
                };

                //____________________________opção1_____________________________________


                //____________________________opção2_____________________________________

                var imgDef = FindViewById<ImageView>(Resource.Id.imgDef);
                var textDef = FindViewById<TextView>(Resource.Id.textDef);

                imgDef.Click += (e, s) => {
                    View Alterarlayout = LayoutInflater.FromContext(this).Inflate(
                    Resource.Layout.LYalterar, null, false);
                    var alert = new AlertDialog.Builder(this)
                    .SetView(Alterarlayout);
                    alert.Show();
                };

                textDef.Click += (e, s) => {

                    View Alterarlayout = LayoutInflater.FromContext(this).Inflate(
                    Resource.Layout.LYalterar, null, false);

                    var alert = new AlertDialog.Builder(this)
                    .SetView(Alterarlayout);
                    alert.Show();
                };

                //____________________________opção2___Relatorio do Professor__________________________________

                //____________________________opção3____Relatorio do Admin_________________________________

                var imgEstatic = FindViewById<ImageView>(Resource.Id.imgEstatic);
                var textEstatic = FindViewById<TextView>(Resource.Id.textEstatic);

                imgEstatic.Click += (e, s) => {
                    try
                    {
                        List<string> nomes = new List<string>(), alunos = new List<string>(), sumario = new List<string>();
                    using (var connect = new SqliteConnection("Data source=" + Intent.GetStringExtra("caminho")))
                    {
                        connect.Open();


                        using (var __SELECTFALTA_ = new SqliteCommand(connect))
                        {
                            __SELECTFALTA_.CommandText = "SELECT * FROM presenca_aluno ";
                            var __readerAluno__ = __SELECTFALTA_.ExecuteReader();
                            while (__readerAluno__.Read())
                            {
                                Toast.MakeText(this, __readerAluno__["falta"].ToString() + "\n" + __readerAluno__["aluno_fk"].ToString() + "\n" + __readerAluno__["sumario"].ToString(), ToastLength.Short).Show();
                                var selectStudents = new SqliteCommand(connect);
                                selectStudents.CommandText = "SELECT * FROM aluno_tb where id_aluno=@id";
                                selectStudents.Parameters.AddWithValue("@id", __readerAluno__["aluno_fk"]);
                                var leitor = selectStudents.ExecuteReader();
                                while (leitor.Read())
                                {
                                    var countFals = new SqliteCommand(connect);
                                    countFals.CommandText = "SELECT count(*) as contador FROM presenca_aluno WHERE aluno_fk=@alfk and falta=@fal ";
                                    countFals.Parameters.AddWithValue("@alfk", leitor["id_aluno"]);
                                    countFals.Parameters.AddWithValue("@fal", "Não");
                                    var carregar = countFals.ExecuteReader();
                                    while (carregar.Read())
                                    {
                                        alunos.Add(leitor["nome"].ToString() + "  |  " + leitor["telefone"] + "  |" + carregar["contador"]);
                                    }

                                    // Jhon Rezzy       | +244992733224 | Física

                                    sumario.Add(__readerAluno__["sumario"].ToString());

                                }

                                var selectProf = new SqliteCommand(connect);
                                selectProf.CommandText = "SELECT * FROM professor_tb where user_fk=@id";
                                selectProf.Parameters.AddWithValue("@id", __readerAluno__["professor_fk"]);
                                var leitor2 = selectProf.ExecuteReader();
                                while (leitor2.Read())
                                {
                                    var TellSelect = new SqliteCommand(connect);
                                    TellSelect.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                    TellSelect.Parameters.AddWithValue("@user", __readerAluno__["professor_fk"]);
                                    var executeTell__ = TellSelect.ExecuteReader();
                                    while (executeTell__.Read())
                                    {
                                        nomes.Add(leitor2["nome"].ToString() + "    |  " + executeTell__["telefone"] + "  | " + leitor2["disciplina_fk"]);
                                    }

                                }

                            }
                            //if (__readerAluno__.HasRows)
                            //{
                            //    Toast.MakeText(this, __readerAluno__["falta"].ToString() + "\n" + __readerAluno__["aluno_fk"].ToString(), ToastLength.Short).Show();

                            //}

                        };
                        connect.Close();
                        connect.Dispose();
                    };
                    View Relatorilayout = LayoutInflater.FromContext(this).Inflate(
                    Resource.Layout.LYadmin_relatorio, null, false);

                    var listaRelatorioA1 = Relatorilayout.FindViewById<ListView>(Resource.Id.listaRelatorioA1);

                    AdminRelatorio relatoriItems = new AdminRelatorio(this, nomes, alunos, sumario);
                    listaRelatorioA1.FastScrollAlwaysVisible = true;
                    listaRelatorioA1.FastScrollEnabled = true;
                    listaRelatorioA1.Adapter = relatoriItems;


                    var listaRelatorioA2 = Relatorilayout.FindViewById<ListView>(Resource.Id.listaRelatorioA2);
                    AdminRelatorioA2 relatoriItemA2 = new AdminRelatorioA2(this, nomes, alunos, sumario);
                    listaRelatorioA2.FastScrollAlwaysVisible = true;
                    listaRelatorioA2.FastScrollEnabled = true;
                    listaRelatorioA2.Adapter = relatoriItems;

                    var alert = new AlertDialog.Builder(this)
                    .SetView(Relatorilayout);
                    alert.Show();
                }
                    catch(System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Short).Show();
                    }
                };

                textEstatic.Click += (e, s) => {
                    try
                    {
                        List<string> nomes = new List<string>(), alunos = new List<string>(), sumario = new List<string>();
                        using (var connect = new SqliteConnection("Data source=" + Intent.GetStringExtra("caminho")))
                        {
                            connect.Open();


                            using (var __SELECTFALTA_ = new SqliteCommand(connect))
                            {
                                __SELECTFALTA_.CommandText = "SELECT * FROM presenca_aluno ";
                                var __readerAluno__ = __SELECTFALTA_.ExecuteReader();
                                while (__readerAluno__.Read())
                                {
                                    Toast.MakeText(this, __readerAluno__["falta"].ToString() + "\n" + __readerAluno__["aluno_fk"].ToString() + "\n" + __readerAluno__["sumario"].ToString(), ToastLength.Short).Show();
                                    var selectStudents = new SqliteCommand(connect);
                                    selectStudents.CommandText = "SELECT * FROM aluno_tb where id_aluno=@id";
                                    selectStudents.Parameters.AddWithValue("@id", __readerAluno__["aluno_fk"]);
                                    var leitor = selectStudents.ExecuteReader();
                                    while (leitor.Read())
                                    {
                                        var countFals = new SqliteCommand(connect);
                                        countFals.CommandText = "SELECT count(*) as contador FROM presenca_aluno WHERE aluno_fk=@alfk and falta=@fal ";
                                        countFals.Parameters.AddWithValue("@alfk", leitor["id_aluno"]);
                                        countFals.Parameters.AddWithValue("@fal", "Não");
                                        var carregar = countFals.ExecuteReader();
                                        while (carregar.Read())
                                        {
                                            alunos.Add(leitor["nome"].ToString() + "  |  " + leitor["telefone"] + "  |" + carregar["contador"]);
                                        }

                                        // Jhon Rezzy       | +244992733224 | Física

                                        sumario.Add(__readerAluno__["sumario"].ToString());

                                    }

                                    var selectProf = new SqliteCommand(connect);
                                    selectProf.CommandText = "SELECT * FROM professor_tb where user_fk=@id";
                                    selectProf.Parameters.AddWithValue("@id", __readerAluno__["professor_fk"]);
                                    var leitor2 = selectProf.ExecuteReader();
                                    while (leitor2.Read())
                                    {
                                        var TellSelect = new SqliteCommand(connect);
                                        TellSelect.CommandText = "SELECT * FROM user_tb WHERE id_user=@user";
                                        TellSelect.Parameters.AddWithValue("@user", __readerAluno__["professor_fk"]);
                                        var executeTell__ = TellSelect.ExecuteReader();
                                        while (executeTell__.Read())
                                        {
                                            nomes.Add(leitor2["nome"].ToString() + "    |  " + executeTell__["telefone"] + "  | " + leitor2["disciplina_fk"]);
                                        }

                                    }

                                }
                                if (__readerAluno__.HasRows)
                                {
                                    Toast.MakeText(this, __readerAluno__["falta"].ToString() + "\n" + __readerAluno__["aluno_fk"].ToString(), ToastLength.Short).Show();

                                }

                            };
                            connect.Close();
                            connect.Dispose();
                        };

                        View Relatorilayout = LayoutInflater.FromContext(this).Inflate(
                         Resource.Layout.LYadmin_relatorio, null, false);

                        var listaRelatorioA1 = Relatorilayout.FindViewById<ListView>(Resource.Id.listaRelatorioA1);

                        AdminRelatorio relatoriItems = new AdminRelatorio(this, nomes, alunos, sumario);
                        listaRelatorioA1.FastScrollAlwaysVisible = true;
                        listaRelatorioA1.FastScrollEnabled = true;
                        listaRelatorioA1.Adapter = relatoriItems;


                        var listaRelatorioA2 = Relatorilayout.FindViewById<ListView>(Resource.Id.listaRelatorioA2);
                        AdminRelatorioA2 relatoriItemA2 = new AdminRelatorioA2(this, nomes, alunos, sumario);
                        listaRelatorioA2.FastScrollAlwaysVisible = true;
                        listaRelatorioA2.FastScrollEnabled = true;
                        listaRelatorioA2.Adapter = relatoriItems;

                        var alert = new AlertDialog.Builder(this)
                        .SetView(Relatorilayout);
                        alert.Show();
                    }catch(System.Exception erro)
                    {
                        Toast.MakeText(this, erro.Message, ToastLength.Short).Show();
                    }
                };

                //____________________________opção3_____________________________________

            }
            else if (conta == "prof")
            {
                int maximo1 = 0;
                SetContentView(Resource.Layout.LYprofessorAula);
                var lista = FindViewById<ListView>(Resource.Id.listView1);
                var menuSetting = FindViewById<ImageButton>(Resource.Id.imageBtnSettings);
                var btnMenu = FindViewById<ImageButton>(Resource.Id.imageBtnMenu);
                var nome_prof = FindViewById<TextView>(Resource.Id.professorNome);
                nome_prof.Text = Intent.GetStringExtra("nome_prof");
                var salario_prof = FindViewById<TextView>(Resource.Id.ProfessorSalario);
                salario_prof.Text = Intent.GetStringExtra("salario_prof") + "$";
                var turma = FindViewById<TextView>(Resource.Id.NomeTurma);
                turma.Text = "Alunos da Turma " + Intent.GetStringExtra("turma_prof");
                var curso = FindViewById<TextView>(Resource.Id.professorCurso);
                curso.Text = Intent.GetStringExtra("curso_prof");

                View SUmarioLayout = LayoutInflater.FromContext(this).Inflate(Resource.Layout.LYsumario, null, false);
                var __sumario__ = SUmarioLayout.FindViewById<EditText>(Resource.Id.__sumario__);
                var __definirSM__ = SUmarioLayout.FindViewById<Button>(Resource.Id.__definir__);
                //var __cancelarSM__ = SUmarioLayout.FindViewById<Button>(Resource.Id.__cancelar__);
                SqliteConnection conexao;
                new AlertDialog.Builder(this)
                    .SetView(SUmarioLayout)
                    .SetNegativeButton("Fechar                           ",
                    (ty, ti) => {

                    })

                    .Show();
                //***********************************Definir sumario evento click**********************************************
                __definirSM__.Click += delegate {

                    using (var con = new SqliteConnection("Data source =" + Intent.GetStringExtra("caminho")))
                    {
                        conexao = con;
                        con.Open();
                        
                        
                        SqliteDataReader reader1;
                        using (var MaxInsert = new SqliteCommand(con))
                        {
                            MaxInsert.CommandText = "SELECT max(id_presenca) as pres FROM presenca_aluno;";
                            reader1 = MaxInsert.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                if (reader1["pres"].ToString() == "")
                                {
                                    maximo1 =1;
                                    //Toast.MakeText(this, " =>caso=> " + reader1["pres"].ToString(), ToastLength.Short).Show();

                                }
                                else if (reader1["pres"].ToString() != "" || int.Parse(reader1["pres"].ToString()) != 0)
                                {
                                    //Toast.MakeText(this," =>caso-> "+reader1["pres"].ToString(), ToastLength.Short).Show();
                                    maximo1 = int.Parse(reader1["pres"].ToString());
                                }
                            }
                        };
                        using (var cmd_alunos = new SqliteCommand(con))
                        {
                            cmd_alunos.CommandText = "SELECT * FROM aluno_tb WHERE turma_fk=@turma";
                            cmd_alunos.Parameters.AddWithValue("@turma", Intent.GetStringExtra("id_turma"));
                            var alunos = cmd_alunos.ExecuteReader();
                            mItems = new List<string>();
                            mtells = new List<string>();
                            mcodes = new List<string>();
                            mfalta = new List<string>();
                            while (alunos.Read())
                            {
                                maximo1++;

                                try
                                {
                                    var inseriFalta = new SqliteCommand(con);

                                    inseriFalta.CommandText = "INSERT INTO presenca_aluno (id_presenca,aluno_fk,professor_fk,sumario,falta,hora_inicio,hora_fim,data) VALUES (@id_presenca,@aluno_fk,@professor_fk,@sumario,@falta,@hora_inicio,@hora_fim,@data);";
                                    inseriFalta.Parameters.AddWithValue("@id_presenca", maximo1);
                                    inseriFalta.Parameters.AddWithValue("@aluno_fk", alunos["id_aluno"]);
                                    inseriFalta.Parameters.AddWithValue("@professor_fk", Intent.GetStringExtra("__id__"));
                                    inseriFalta.Parameters.AddWithValue("@sumario", __sumario__.Text);
                                    inseriFalta.Parameters.AddWithValue("@falta", "Não");
                                    inseriFalta.Parameters.AddWithValue("@hora_inicio", Intent.GetStringExtra("hora_inicio"));
                                    inseriFalta.Parameters.AddWithValue("@hora_fim", Intent.GetStringExtra("hora_final"));
                                    inseriFalta.Parameters.AddWithValue("@data", new DatePicker(this).DateTime.ToShortDateString());
                                    inseriFalta.ExecuteNonQuery();
                                }
                                catch (System.Exception erro)
                                {
                                    Toast.MakeText(this, erro.Message, ToastLength.Short).Show();
                                }

                                //mItems.Add(alunos["nome"].ToString());
                                //mcodes.Add(alunos["id_aluno"].ToString());
                                //mtells.Add(alunos["telefone"].ToString());

                                //Toast.MakeText(this, Intent.GetStringExtra("hora_inicio")+" \n"+Intent.GetStringExtra("__id__") +"\n"+Intent.GetStringExtra("hora_final"), ToastLength.Short).Show();
                            }
                            Toast.MakeText(this,"Sumario definido .já podes fechar a janela", ToastLength.Short).Show();
                        }
                        con.Close();
                        con.Dispose();
                    }
                        };
                 //*********************************fim definir sumario e evnto click***************************************************


                        menuSetting.Click += (s, e) =>
                        {
                            PopupMenu pop = new PopupMenu(this, menuSetting);
                            pop.MenuInflater.Inflate(Resource.Layout.menuIt, pop.Menu);
                            pop.Show();
                            pop.MenuItemClick += (ss, ee) =>
                            {
                                if (ee.Item.ToString() == "Sair")
                                {
                                    this.Finish();
                                    var AT_login = new Intent(this, typeof(ATsplash));
                                    AT_login.PutExtra("caminho", Intent.GetStringExtra("caminho"));
                                    StartActivity(AT_login);
                                }
                            };

                        };


                        using (var connect = new SqliteConnection("Data source =" + Intent.GetStringExtra("caminho")))
                        {
                            conexao = connect;
                            connect.Open();
                            int maximo = 0;
                            SqliteDataReader reader;
                            using (var cmd_alunos = new SqliteCommand(connect))
                            {
                                cmd_alunos.CommandText = "SELECT * FROM aluno_tb WHERE turma_fk=@turma";
                                cmd_alunos.Parameters.AddWithValue("@turma", Intent.GetStringExtra("id_turma"));
                                var alunos = cmd_alunos.ExecuteReader();
                                mItems = new List<string>();
                                mtells = new List<string>();
                                mcodes = new List<string>();
                                while (alunos.Read())
                                {
                                    

                                    mItems.Add(alunos["nome"].ToString());
                                    mcodes.Add(alunos["id_aluno"].ToString());
                                    mtells.Add(alunos["telefone"].ToString());
                            //var __getFalta__ = new SqliteCommand(connect);
                            //__getFalta__.CommandText = "SELECT count(*) as contador FROM presenca_aluno WHERE aluno_fk=@al and falta=@fa";
                            //__getFalta__.Parameters.AddWithValue("@al",alunos["id_aluno"]);
                            //__getFalta__.Parameters.AddWithValue("@fa", "Não");
                            //var __launchResults__ = __getFalta__.ExecuteReader();
                            //while (__launchResults__.Read())
                            //{
                            //    mfalta.Add(__launchResults__["contador"].ToString());
                            //}

                            //Toast.MakeText(this, Intent.GetStringExtra("hora_inicio")+" \n"+Intent.GetStringExtra("__id__") +"\n"+Intent.GetStringExtra("hora_final"), ToastLength.Short).Show();
                        }
                                
                            };

                   
                            connect.Close();
                            connect.Dispose();
                        }



                        //mItems = new List<string>() { "John Sizzy","Ramy Malek","Morgan Nachrt",
                        //"Johan Trejo","Antoniye Yegout","Monica Alcateia","Maey Hamed Zechs",};
                        ProfList professor = new ProfList(this, mItems, mcodes, mtells);
                        professor.GetDataBase(conexao);//enviar conexão para lista de professores
                professor.SetElements(new DatePicker(this).DateTime.ToShortDateString(), Intent.GetStringExtra("hora_inicio"), Intent.GetStringExtra("hora_final"));


                        lista.SetMinimumWidth((lista.Width * mItems.Count) + lista.Width);
                        lista.Adapter = professor;
                        lista.FastScrollEnabled = true;

                        //terminar o tempo  do professor actual******************************************************
                        string[] ArrayHour = Intent.GetStringExtra("hora_final").Split(':');
                        tmp.Interval = 1000;
                        tmp.Start();
                        tmp.Elapsed += (tm, tn) => {

                            int hora_Sys = new TimePicker(this).CurrentHour.IntValue();
                            int minuto_Sys = new TimePicker(this).CurrentMinute.IntValue();

                            int hora_Log = int.Parse(ArrayHour[0]);
                            int minuto_Log = int.Parse(ArrayHour[1]);

                            RunOnUiThread(() => {

                                if (hora_Sys == hora_Log && (minuto_Sys >= minuto_Log + 1 && minuto_Sys <= minuto_Log + 3))
                                {
                                    tmp.Stop();
                                    new AlertDialog.Builder(this)
                                    .SetMessage("Hora de terminar " + nome_prof.Text)
                                    .Show();
                                    var It = new Intent(this, typeof(ATlogin));
                                    It.PutExtra("caminho", Intent.GetStringExtra("caminho"));
                                    StartActivity(It);
                                    this.Finish();

                                }


                            });
                        };
                //terminar o tempo  do professor actual******************************************************


            }

        }
    }
}


        
    