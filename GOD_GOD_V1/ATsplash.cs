using Android.App;
using Android.Widget;
using Android.OS;
using System.Timers;
using Android.Views;
using System.IO;
using Mono.Data.Sqlite;
using Android.Content;
using Android.Telephony;
using System.Collections.Generic;
namespace GOD_GOD_V1
{
    [Activity(Label = "Appointment Scheduler app ", MainLauncher = true)]
    #region ATSplashClassStart
    public class ATsplash : Activity
    {
        public string Folder
        { get; set;}

        Timer tempo = new Timer();
        //bool atraso;
        string BaseDados;
        //SqliteConnection connect;
        #region OncreateStart
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LYsplash);

            // Set our view from the "main" layout resource
            #region tRYccreatDatabase

            try
            {
              string Folder = Environment.ExternalStorageDirectory.AbsolutePath;
              this.Folder = Folder = Path.Combine(Folder, "SGPdatabase");
              BaseDados = Path.Combine(Folder, "sgp_v1.db");
               
                CreateDatabase db = new CreateDatabase(Folder, BaseDados);

                string queryUser = "CREATE TABLE  user_tb( " +
                    "id_user INTEGER NOT NULL PRIMARY KEY," +
                    "telefone  VARCHAR(15)," +
                    "senha VARCHAR(255)," +
                    "tipo_conta  VARCHAR(12)" +
                    " );";
                string queryProfessor = "CREATE TABLE  professor_tb( " +
                    "id_prof INTEGER NOT NULL PRIMARY KEY," +
                    "nome  VARCHAR(30)," +
                    "genero  VARCHAR(12)," +
                    "salario  FLOAT," +
                    "nascimento DATE," +
                    "disciplina_fk INTEGER," +
                    "user_fk INTEGER ," +
                    "FOREIGN KEY (user_fk) REFERENCES user_tb(id_user)," +
                    "FOREIGN KEY (disciplina_fk) REFERENCES disciplina_tb(id_disciplina)" +
                    " );";

                string queryDisciplina = "CREATE TABLE  disciplina_tb( " +
                    "id_disciplina INTEGER NOT NULL PRIMARY KEY," +
                    "disciplina  VARCHAR(30)," +
                    "direitor_fk  INTEGER," +
                    "curso_fk  INTEGER," +
                    "FOREIGN KEY (direitor_fk) REFERENCES direitor_tb(id_direitor)," +
                    "FOREIGN KEY (curso_fk) REFERENCES curso_tb(id_curso)" +
                    "  );";

                string queryAluno = "CREATE TABLE  aluno_tb( " +
                    "id_aluno INTEGER NOT NULL PRIMARY KEY," +
                    "nome  VARCHAR(30)," +
                    "genero  VARCHAR(12)," +
                    "nascimento DATE," +
                    "telefone  VARCHAR(15)," +
                    "turma_fk INTEGER," +
                    "FOREIGN KEY (turma_fk) REFERENCES turma_tb(id_turma)" +
                    "  );";
                string queryTurma = "CREATE TABLE  turma_tb( " +
                    "id_turma INTEGER NOT NULL PRIMARY KEY," +
                    "turma VARCHAR(30)," +
                    "curso_fk  INTEGER," +
                    "classe VARCHAR(30)," +
                    "FOREIGN KEY (curso_fk) REFERENCES curso_tb(id_curso)" +
                    ");";
                string queryCurso = "CREATE TABLE  curso_tb( " +
                   "id_curso INTEGER NOT NULL PRIMARY KEY," +
                   "curso VARCHAR(30)," +
                   "direitor_fk  INTEGER," +
                   "FOREIGN KEY (direitor_fk) REFERENCES direitor_tb (id_direitor)"
                   + "  );";
                string queryDireitor = "CREATE TABLE  direitor_tb( " +
                   "id_direitor INTEGER NOT NULL PRIMARY KEY," +
                   "nome VARCHAR(30)," +
                   "telefone  VARCHAR(30)," +
                   "nascimento  DATE," +
                   "user_fk INTEGER," +
                   "FOREIGN KEY (user_fk) REFERENCES user_tb(id_user)" +
                   " );";
                string queryHorario = "CREATE TABLE  horario_tb( " +
                   "id_horario INTEGER NOT NULL PRIMARY KEY," +
                   "hora_inicio varchar(15)," +
                   "hora_fim  varcha(15)," +
                   "dia       VARCHAR(26)," +
                   "turma_fk  INTEGER," +
                   "periodo VARCHAR(30)," +
                   "disciplina_fk INTEGER," +
                   "FOREIGN KEY (disciplina_fk) REFERENCES disciplina_tb(id_disciplina)," +
                   "FOREIGN KEY (turma_fk) REFERENCES turma_tb(id_turma)" +
                   " );";
                string queryMensagem = "CREATE TABLE  mensagem_tb( " +
                   "id_sms INTEGER NOT NULL PRIMARY KEY," +
                   "origem INTEGER," +
                   "destino  INTEGER," +
                   "texto  TEXT," +
                   "FOREIGN KEY (origem) REFERENCES professor_tb(id_prof)," +
                   "FOREIGN KEY (destino) REFERENCES direitor_tb (id_direitor)" +
                   " );";
                string __PROF__ = "CREATE TABLE  presenca_prof( " +
                   "id_presenca INTEGER NOT NULL PRIMARY KEY," +
                   "professor_fk INTEGER," +
                   "motivo VARCHAR(50)," +
                   "falta VARCHAR(500)," +
                   "hora_inicio  VARCHAR(10)," +
                   "hora_fim  VARCHAR(10)," +
                   "data VARCHAR(15)," +
                   "FOREIGN KEY (professor_fk) REFERENCES professor_tb(id_prof)"+
                   "  );";

                string __ALUNO__ = "CREATE TABLE  presenca_aluno( " +
                  "id_presenca INTEGER NOT NULL PRIMARY KEY," +
                  "aluno_fk INTEGER," +
                  "professor_fk INTEGER," +
                  "sumario VARCHAR(500)," +
                  "falta VARCHAR(500)," +
                  "hora_inicio  VARCHAR(10)," +
                  "hora_fim  VARCHAR(10)," +
                  "data VARCHAR(15)," +
                  "FOREIGN KEY (professor_fk) REFERENCES professor_tb(id_prof)" +
                  "  );";

                db.CDatabase(Folder, BaseDados, queryAluno);
                db.CDatabase(Folder, BaseDados, queryUser);
                db.CDatabase(Folder, BaseDados, queryProfessor);
                db.CDatabase(Folder, BaseDados, queryDisciplina);
                db.CDatabase(Folder, BaseDados, queryHorario);
                db.CDatabase(Folder, BaseDados,__ALUNO__);
                db.CDatabase(Folder, BaseDados,__PROF__);
                db.CDatabase(Folder, BaseDados, queryTurma);
                db.CDatabase(Folder, BaseDados, queryCurso);
                db.CDatabase(Folder, BaseDados, queryMensagem);
                db.CDatabase(Folder, BaseDados, queryDireitor);
                Toast.MakeText(this, "Bem-vindo ao Appointment book", ToastLength.Short)
                   .Show();
                 
                try
                {
                    db.InsertOnProfessorTB(BaseDados);
                    db.InsertOnHorario12TA(BaseDados);//bom
                        db.InsertOnHorario11MA(BaseDados);
                        db.InsertOnUserTb(BaseDados);
                        db.InsertOnDireitorTB(BaseDados);
                        db.InsertOnCursoTB(BaseDados);
                        db.InsertOnDisciplinaTB(BaseDados);
                        db.InsertOnTurmaTB(BaseDados);
                        db.InsertOnAlunoTB(BaseDados);
                        
                       
                   
                    //db.InsertOnUserTb(BaseDados);
                   
                }
                catch (System.Exception eror)
                {
                    Toast.MakeText(this, "Seje bem-vindo ao Point Book", ToastLength.Short)
                   .Show();
                    //var Alert = new AlertDialog.Builder(this);
                    //Alert.SetMessage(eror.Message);
                    //Alert.Show();
                }
               
               
            }
            catch (System.Exception erro)
            {
                var alerta = new AlertDialog.Builder(this);
                alerta.SetMessage(erro.Message);
                alerta.Show();
            }

            #endregion

            var btn_entrar = FindViewById<Button>(Resource.Id.btu);
            var timer = FindViewById<TimePicker>(Resource.Id.timer);
            var dater = FindViewById<DatePicker>(Resource.Id.dater);
            int count = 0;
           // string periodo;
            tempo.Start();
            

            btn_entrar.Click += (evento, sender) =>
            {

                var AT_login = new Intent(this, typeof(ATlogin));
                AT_login.PutExtra("caminho", BaseDados);
                StartActivity(AT_login);
                tempo.Stop();


            };

            #region ElapsedAreaStart


            tempo.Elapsed += (sender, evento) =>
        {
            if (count < 100)
            {
                RunOnUiThread(() =>
                {
                    count++;
                    if (count == 9)
                    {
                    }
                });
            }
        };


            #endregion ElapsedAreaEnd
        }

        #endregion OncreateEnd
    }

    #endregion ATSplashClassEnd



    #region CreateDataBaseStart

    public class CreateDatabase
    {
        public string ConnectionStringBuilder;
        public System.Exception Error;
        private string caminhoCompleto;

        public CreateDatabase(string Pasta, string DataBase)
        {
            if (!Directory.Exists(Pasta))
            {

                Directory.CreateDirectory(Pasta);
                var dbDataBase = Path.Combine(Pasta, DataBase);
                caminhoCompleto = dbDataBase;

                if (!File.Exists(dbDataBase))
                {
                    SqliteConnection.CreateFile(DataBase);
                    ConnectionStringBuilder = dbDataBase;
                    caminhoCompleto = dbDataBase;
                }

            }
        }
        public void CDatabase(string Pasta, string Database, string TableStruct)
        {
            if (Directory.Exists(Pasta))
            {

                var dbDatabase = Path.Combine(Pasta, Database);
                if (!File.Exists(dbDatabase))
                {
                    SqliteConnection.CreateFile(dbDatabase);
                    ConnectionStringBuilder = dbDatabase;
                }

                if (File.Exists(dbDatabase))
                {


                    using (var connect = new SqliteConnection("Data source =" + dbDatabase))
                    {
                        connect.Open();
                        using (var trans = connect.BeginTransaction())
                        {
                            using (var cmd = new SqliteCommand())
                            {
                                try
                                {
                                    cmd.Connection = connect;
                                    cmd.CommandText = TableStruct;
                                    cmd.ExecuteNonQuery();
                                }
                                catch (System.Exception erro)
                                {
                                    Error = erro;
                                }
                            }

                            trans.Commit();
                        }
                        connect.Close();
                        connect.Dispose();
                    }

                }

            }

        }


        public void insertData(string caminho)
        {
            string BaseDados = caminho;

            using (var connect = new SqliteConnection("Data source=" + BaseDados))
            {
                using (var trans = connect.BeginTransaction())
                {


                    using (var cmd = new SqliteCommand(connect))
                    {

                        #region USER_TB
                       // cmd.CommandText =
                       //"INSERT INTO user_tb(id_user,telefone,senha,tipo_conta)" +
                       //"VALUES (@id_user,@telefone,@senha,@tipo_conta)";

                       // cmd.Parameters.AddWithValue("@id_user", 1);
                       // cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                       // cmd.Parameters.AddWithValue("@senha", "pensador43");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "admin");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 2);
                       // cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                       // cmd.Parameters.AddWithValue("@senha", "leonel");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "admin");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 3);
                       // cmd.Parameters.AddWithValue("@telefone", "+244911457117");
                       // cmd.Parameters.AddWithValue("@senha", "manuel");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 4);
                       // cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                       // cmd.Parameters.AddWithValue("@senha", "vemba");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "admin");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 5);
                       // cmd.Parameters.AddWithValue("@telefone", "+244931926436");
                       // cmd.Parameters.AddWithValue("@senha", "ramiro");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "admin");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 6);
                       // cmd.Parameters.AddWithValue("@telefone", "+244947682919");
                       // cmd.Parameters.AddWithValue("@senha", "faustino");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 7);
                       // cmd.Parameters.AddWithValue("@telefone", "+244939212554");
                       // cmd.Parameters.AddWithValue("@senha", "bento");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 8);
                       // cmd.Parameters.AddWithValue("@telefone", "+244992733226");
                       // cmd.Parameters.AddWithValue("@senha", "lezadomingos");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 9);
                       // cmd.Parameters.AddWithValue("@telefone", "+244912766637");
                       // cmd.Parameters.AddWithValue("@senha", "mario");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 10);
                       // cmd.Parameters.AddWithValue("@telefone", "+244912242750");
                       // cmd.Parameters.AddWithValue("@senha", "costa");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 11);
                       // cmd.Parameters.AddWithValue("@telefone", "+244939005616");
                       // cmd.Parameters.AddWithValue("@senha", "silvina");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 12);
                       // cmd.Parameters.AddWithValue("@telefone", "+244912447222");
                       // cmd.Parameters.AddWithValue("@senha", "eduinarocha");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 13);
                       // cmd.Parameters.AddWithValue("@telefone", "+244924117597");
                       // cmd.Parameters.AddWithValue("@senha", "josemar");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 14);
                       // cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                       // cmd.Parameters.AddWithValue("@senha", "Abel");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 15);
                       // cmd.Parameters.AddWithValue("@telefone", "+244993356447");
                       // cmd.Parameters.AddWithValue("@senha", "luizviana12");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 16);
                       // cmd.Parameters.AddWithValue("@telefone", "+244943187545");
                       // cmd.Parameters.AddWithValue("@senha", "simao65");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();

                       // cmd.Parameters.AddWithValue("@id_user", 17);
                       // cmd.Parameters.AddWithValue("@telefone", "+244997219116");
                       // cmd.Parameters.AddWithValue("@senha", "victorinomussoque");
                       // cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                       // cmd.ExecuteNonQuery();
                        #endregion

                        #region DIRETITOR_TB
                        //cmd.CommandText =
                        //"INSERT INTO direitor_tb(id_direito, nome, telefone, nascimento, user_fk)" +
                        //"VALUES(@id_direitor,@nome,@telefone,@nascimento,@user_fk)";

                        //cmd.Parameters.AddWithValue("@id_direitor", 1);
                        //cmd.Parameters.AddWithValue("@nome", "Santos");
                        //cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                        //cmd.Parameters.AddWithValue("@nascimento", "1996-12-18");
                        //cmd.Parameters.AddWithValue("@user_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_direitor", 2);
                        //cmd.Parameters.AddWithValue("@nome", "Angelo");
                        //cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                        //cmd.Parameters.AddWithValue("@nascimento", "1998-19-02");
                        //cmd.Parameters.AddWithValue("@user_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_direitor", 3);
                        //cmd.Parameters.AddWithValue("@nome", "Malaquias");
                        //cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                        //cmd.Parameters.AddWithValue("@nascimento", "2000-18-21");
                        //cmd.Parameters.AddWithValue("@user_fk", 4);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_direitor", 4);
                        //cmd.Parameters.AddWithValue("@nome", "Ramiro");
                        //cmd.Parameters.AddWithValue("@telefone", "+244931926436");
                        //cmd.Parameters.AddWithValue("@nascimento", "1985-21-04");
                        //cmd.Parameters.AddWithValue("@user_fk", 5);
                        //cmd.ExecuteNonQuery();
                        #endregion
                        
                        #region CURSO TB
                    //    cmd.CommandText =
                    //"INSERT INTO curso_tb(id_curso,curso, direitor_fk)" +
                    //"VALUES(@id_curso,@curso,@direitor_fk)";

                    //    cmd.Parameters.AddWithValue("@id_curso", 1);
                    //    cmd.Parameters.AddWithValue("@curso", "Tecnico de Informatica");
                    //    cmd.Parameters.AddWithValue("@direitor_fk", 2);
                    //    cmd.ExecuteNonQuery();

                    //    cmd.Parameters.AddWithValue("@id_curso", 2);
                    //    cmd.Parameters.AddWithValue("@curso", "Desenhador Projetista");
                    //    cmd.Parameters.AddWithValue("@direitor_fk", 1);
                    //    cmd.ExecuteNonQuery();

                    //    cmd.Parameters.AddWithValue("@id_curso", 3);
                    //    cmd.Parameters.AddWithValue("@curso", "Contabilidade");
                    //    cmd.Parameters.AddWithValue("@direitor_fk", 3);
                    //    cmd.ExecuteNonQuery();

                    //    cmd.Parameters.AddWithValue("@id_curso", 4);
                    //    cmd.Parameters.AddWithValue("@curso", "Informatica de gestao");
                    //    cmd.Parameters.AddWithValue("@direitor_fk", 5);
                    //    cmd.ExecuteNonQuery();

                    //    cmd.Parameters.AddWithValue("@id_curso", 5);
                    //    cmd.Parameters.AddWithValue("@curso", "Contruicao civil");
                    //    cmd.Parameters.AddWithValue("@direitor_fk", 4);
                    //    cmd.ExecuteNonQuery();
                        #endregion
                        #region DISCIPLINA_TB
                        //cmd.CommandText =
                        //"INSERT INTO disciplina_tb (id_disciplina,disciplina,direitor_fk,curso_fk)" +
                        //"VALUES (@id_disciplina,@disciplina,@direitor_fk,@curso_fk)";

                        //cmd.Parameters.AddWithValue("@id_disciplina", 1);
                        //cmd.Parameters.AddWithValue("@disciplina", "Matemática");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 1);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 2);
                        //cmd.Parameters.AddWithValue("@disciplina", "TLP");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 2);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 3);
                        //cmd.Parameters.AddWithValue("@disciplina", "TREI");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 4);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 4);
                        //cmd.Parameters.AddWithValue("@disciplina", "Física");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 3);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 5);
                        //cmd.Parameters.AddWithValue("@disciplina", "SEAC");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 2);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 6);
                        //cmd.Parameters.AddWithValue("@disciplina", "PT");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 3);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();
                        ////fim tarde

                        ////inicio Manhã
                        //cmd.Parameters.AddWithValue("@id_disciplina", 7);
                        //cmd.Parameters.AddWithValue("@disciplina", "Matemática");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 2);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 8);
                        //cmd.Parameters.AddWithValue("@disciplina", "Física");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 4);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 9);
                        //cmd.Parameters.AddWithValue("@disciplina", "Lingua Portuguesa");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 1);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 10);
                        //cmd.Parameters.AddWithValue("@disciplina", "TLP");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 2);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 11);
                        //cmd.Parameters.AddWithValue("@disciplina", "TIC");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 3);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 12);
                        //cmd.Parameters.AddWithValue("@disciplina", "SEAC");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 1);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 13);
                        //cmd.Parameters.AddWithValue("@disciplina", "Língua Inlgêsa");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 2);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 14);
                        //cmd.Parameters.AddWithValue("@disciplina", "FAI");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 3);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 15);
                        //cmd.Parameters.AddWithValue("@disciplina", "EDfísica");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 3);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 16);
                        //cmd.Parameters.AddWithValue("@disciplina", "Electrotecnia");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 4);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 17);
                        //cmd.Parameters.AddWithValue("@disciplina", "OGI");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 2);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_disciplina", 18);
                        //cmd.Parameters.AddWithValue("@disciplina", "Fisisca");
                        //cmd.Parameters.AddWithValue("@direitor_fk", 4);
                        //cmd.Parameters.AddWithValue("@curso_fk", 1);
                        //cmd.ExecuteNonQuery();
                        #endregion
                        
                        #region PROFESSOR_TB 17
                        //cmd.CommandText = "INSERT INTO professor_tb (id_prof,nome,genero,salario,nascimento,disciplina_fk,user_fk ) " +
                        //"VALUES(@id_prof,@nome,@genero,@salario,@nascimento,@disciplina_fk,@user_fk)";
                        //cmd.Parameters.AddWithValue("@id_prof", 1);
                        //cmd.Parameters.AddWithValue("@nome", "Santos Ferreira Campos");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 530.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                        //cmd.Parameters.AddWithValue("@user_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 2);
                        //cmd.Parameters.AddWithValue("@nome", "Emanuel Ngola");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                        //cmd.Parameters.AddWithValue("@user_fk", 3);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 3);
                        //cmd.Parameters.AddWithValue("@nome", "Ramiro Cardoso");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                        //cmd.Parameters.AddWithValue("@user_fk", 5);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 4);
                        //cmd.Parameters.AddWithValue("@nome", "Angelo Leonel Da costa");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 230.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 4);
                        //cmd.Parameters.AddWithValue("@user_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 5);
                        //cmd.Parameters.AddWithValue("@nome", "Bento Pedro");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 5);
                        //cmd.Parameters.AddWithValue("@user_fk", 7);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 6);
                        //cmd.Parameters.AddWithValue("@nome", "Faustino Samaiavo");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                        //cmd.Parameters.AddWithValue("@user_fk", 6);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 7);
                        //cmd.Parameters.AddWithValue("@nome", "Domingos Leza ");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 230.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 9);
                        //cmd.Parameters.AddWithValue("@user_fk", 8);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 8);
                        //cmd.Parameters.AddWithValue("@nome", "Mario Emilio");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 12);
                        //cmd.Parameters.AddWithValue("@user_fk", 9);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 9);
                        //cmd.Parameters.AddWithValue("@nome", "António da Costa");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 11);
                        //cmd.Parameters.AddWithValue("@user_fk", 10);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 10);
                        //cmd.Parameters.AddWithValue("@nome", "Silvína C. Silvina");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 14);
                        //cmd.Parameters.AddWithValue("@user_fk", 11);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 11);
                        //cmd.Parameters.AddWithValue("@nome", "Eduina da Rocha");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                        //cmd.Parameters.AddWithValue("@user_fk", 12);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 12);
                        //cmd.Parameters.AddWithValue("@nome", "José de Jesus Pedro");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 230.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 15);
                        //cmd.Parameters.AddWithValue("@user_fk", 13);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 13);
                        //cmd.Parameters.AddWithValue("@nome", "Eduardo Abel");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 15);
                        //cmd.Parameters.AddWithValue("@user_fk", 14);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 14);
                        //cmd.Parameters.AddWithValue("@nome", "Luiz Viana");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 16);
                        //cmd.Parameters.AddWithValue("@user_fk", 15);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 15);
                        //cmd.Parameters.AddWithValue("@nome", "Simao Diogo");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 15);
                        //cmd.Parameters.AddWithValue("@user_fk", 16);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 16);
                        //cmd.Parameters.AddWithValue("@nome", "Domingos Leza");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 330.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 14);
                        //cmd.Parameters.AddWithValue("@user_fk", 15);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_prof", 17);
                        //cmd.Parameters.AddWithValue("@nome", "Victor Mussoque");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@salario", 230.000);
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@disciplina_fk", 16);
                        //cmd.Parameters.AddWithValue("@user_fk", 17);
                        //cmd.ExecuteNonQuery();

                        //turma
                        #endregion 17

                        #region  HORARIO_TB
                        cmd.CommandText =
                        "INSERT INTO horario_tb (id_horario,hora_inicio,hora_fim,dia,turma_fk,periodo,disciplina_fk) " +
                        "VALUES(@id_horario,@hora_inicio,@hora_fim,@dia,@turma_fk,@periodo,@disciplina_fk)";


                        //segunda
                        cmd.Parameters.AddWithValue("@id_horario", 1);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 2);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 5);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 3);
                        cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 4);
                        cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                        cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@di sciplina_fk", 3);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 5);
                        cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                        cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 6);
                        cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                        cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                        cmd.ExecuteNonQuery();
                        //fiim segunda

                        //terça-feora
                        cmd.Parameters.AddWithValue("@id_horario", 7);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 17);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 8);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 17);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 9);
                        cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 10);
                        cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                        cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 11);
                        cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                        cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 12);
                        cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                        cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                        cmd.ExecuteNonQuery();

                        //fim terça feira

                        //quarta-feira inicio

                        cmd.Parameters.AddWithValue("@id_horario", 13);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 8);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 14);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 8);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 15);
                        cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 16);
                        cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                        cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@di sciplina_fk", 2);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 17);
                        cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                        cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 18);
                        cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                        cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                        cmd.ExecuteNonQuery();
                        //fim quarta feira

                        //quinta feira

                        cmd.Parameters.AddWithValue("@id_horario", 19);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                        cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 8);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 20);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                        cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 8);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 21);
                        cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                        cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 22);
                        cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                        cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                        cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@di sciplina_fk", 3);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 23);
                        cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                        cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                        cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 24);
                        cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                        cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                        cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                        cmd.ExecuteNonQuery();

                        //fim quinta feira

                        //sexta feira

                        cmd.Parameters.AddWithValue("@id_horario", 25);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 26);
                        cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 27);
                        cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 28);
                        cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                        cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@di sciplina_fk", 3);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 29);
                        cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                        cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 5);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 30);
                        cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                        cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 1);
                        cmd.Parameters.AddWithValue("@periodo", "Tarde");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 5);
                        cmd.ExecuteNonQuery();

                        //fim sexta feira


                        //_____________________________________Manhã_______________________________________________________


                        //segunda
                        cmd.Parameters.AddWithValue("@id_horario", 31);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk",2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 4);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 32);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk",2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 4);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 33);
                        cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 11);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 34);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@di sciplina_fk",11);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 35);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk",1);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 36);
                        cmd.Parameters.AddWithValue("@hora_inicio", "11:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "12:40");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                        cmd.ExecuteNonQuery();
                        //fiim segunda

                        //terça-feora
                        cmd.Parameters.AddWithValue("@id_horario", 37);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 38);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 39);
                        cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 13);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 40);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@di sciplina_fk",6);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 41);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 14);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 42);
                        cmd.Parameters.AddWithValue("@hora_inicio", "11:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "12:40");
                        cmd.Parameters.AddWithValue("@dia", "terça-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 14);
                        cmd.ExecuteNonQuery();

                        //fim terça feira

                        //quarta-feira inicio
                        cmd.Parameters.AddWithValue("@id_horario", 43);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 5);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 44);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 5);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 45);
                        cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 46);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                        cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@di sciplina_fk", 6);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 47);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 13);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 48);
                        cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                        cmd.Parameters.AddWithValue("@hora_fim", "11:50");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 13);
                        cmd.ExecuteNonQuery();
                        //fim quarta feira

                        //quinta feira
                        cmd.Parameters.AddWithValue("@id_horario", 49);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                        cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk",4);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 50);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                        cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 4);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 51);
                        cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk",2);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 52);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@di sciplina_fk",2);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 53);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk",1);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 54);
                        cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                        cmd.Parameters.AddWithValue("@hora_fim", "11:50");
                        cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk",1);
                        cmd.ExecuteNonQuery();

                        //fim quinta feira

                        //sexta feira
                        cmd.Parameters.AddWithValue("@id_horario", 55);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 16);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 56);
                        cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk",16);
                        cmd.ExecuteNonQuery();



                        cmd.Parameters.AddWithValue("@id_horario", 57);
                        cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                        cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk",2);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 58);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                        cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@di sciplina_fk", 2);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 59);
                        cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                        cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk", 11);
                        cmd.ExecuteNonQuery();


                        cmd.Parameters.AddWithValue("@id_horario", 60);
                        cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                        cmd.Parameters.AddWithValue("@hora_fim", "11:50");
                        cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                        cmd.Parameters.AddWithValue("@turma_fk", 2);
                        cmd.Parameters.AddWithValue("@periodo", "Manhã");
                        cmd.Parameters.AddWithValue("@disciplina_fk",11);
                        cmd.ExecuteNonQuery();


                        #endregion
                        #region ALUNO_TB
                        //cmd.CommandText = "INSERT INTO aluno_tb (id_aluno,nome,genero,nascimento,telefone,turma_fk) " +
                        //    "VALUES(@id_aluno,@nome,@genero,@nascimento,@telefone,@turma_fk)";


                        ////TURMA TI12MA-----------------------------------------------------------1
                        //cmd.Parameters.AddWithValue("@id_aluno", 1);
                        //cmd.Parameters.AddWithValue("@nome", "Root danke");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244947682919");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 2);
                        //cmd.Parameters.AddWithValue("@nome", "Jessienh haus");
                        //cmd.Parameters.AddWithValue("@genero", "Feminino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244939212554");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 3);
                        //cmd.Parameters.AddWithValue("@nome", "Analia de Jesus");
                        //cmd.Parameters.AddWithValue("@genero", "Feminino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 4);
                        //cmd.Parameters.AddWithValue("@nome", "Sechs Nacht");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244912766637");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 5);
                        //cmd.Parameters.AddWithValue("@nome", "Iron Morgen");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244992733226");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 6);
                        //cmd.Parameters.AddWithValue("@nome", "Bitte Slecht");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244912442750");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 7);
                        //cmd.Parameters.AddWithValue("@nome", "Cehna Wiedersehn");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244939005616");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 8);
                        //cmd.Parameters.AddWithValue("@nome", "Adam Smith");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244912447222");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 9);
                        //cmd.Parameters.AddWithValue("@nome", "Alan keymore");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244924117597");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 10);
                        //cmd.Parameters.AddWithValue("@nome", "Monich via");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 11);
                        //cmd.Parameters.AddWithValue("@nome", "Ramiro Tschss");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244911457117");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 12);
                        //cmd.Parameters.AddWithValue("@nome", "Jessica da Costa");
                        //cmd.Parameters.AddWithValue("@genero", "Feminino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 13);
                        //cmd.Parameters.AddWithValue("@nome", "Roberta Miranda");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 14);
                        //cmd.Parameters.AddWithValue("@nome", "Gray NickMcbone");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244931926436");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 15);
                        //cmd.Parameters.AddWithValue("@nome", "Backbone Mcmc");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244992733226");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 16);
                        //cmd.Parameters.AddWithValue("@nome", "Thiago Silva");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+224912242750");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 17);
                        //cmd.Parameters.AddWithValue("@nome", "Motta MochtMorgen");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                        //cmd.Parameters.AddWithValue("@turma_fk", 1);
                        //cmd.ExecuteNonQuery();

                        ////TURMA TI10MA-----------------------------------------------------------2
                        //cmd.Parameters.AddWithValue("@id_aluno", 18);
                        //cmd.Parameters.AddWithValue("@nome", "Roobert Kehn");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 19);
                        //cmd.Parameters.AddWithValue("@nome", "Jessey Adam");
                        //cmd.Parameters.AddWithValue("@genero", "Feminino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 20);
                        //cmd.Parameters.AddWithValue("@nome", "Josemar cristovão");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 21);
                        //cmd.Parameters.AddWithValue("@nome", "Bossac Mcmore");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 22);
                        //cmd.Parameters.AddWithValue("@nome", "Ihr Sehr Mag");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 23);
                        //cmd.Parameters.AddWithValue("@nome", "Mutter sind");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 24);
                        //cmd.Parameters.AddWithValue("@nome", "Wiedersehn Haben");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 25);
                        //cmd.Parameters.AddWithValue("@nome", "Adam Has stress");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 26);
                        //cmd.Parameters.AddWithValue("@nome", "Domingos Armando");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 27);
                        //cmd.Parameters.AddWithValue("@nome", "Valeria Aynna");
                        //cmd.Parameters.AddWithValue("@genero", "Femenino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 28);
                        //cmd.Parameters.AddWithValue("@nome", "Roobertt Tschss");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                        //cmd.Parameters.AddWithValue("@turma_fk", "+244927515922");
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 29);
                        //cmd.Parameters.AddWithValue("@nome", "Jessica da Costa");
                        //cmd.Parameters.AddWithValue("@genero", "Feminino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 30);
                        //cmd.Parameters.AddWithValue("@nome", "Roberta Straass Miranda");
                        //cmd.Parameters.AddWithValue("@genero", "Femenino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 31);
                        //cmd.Parameters.AddWithValue("@nome", "Gray Haus NickMcbone");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 32);
                        //cmd.Parameters.AddWithValue("@nome", "Backbone Mc. Mcmc");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 33);
                        //cmd.Parameters.AddWithValue("@nome", "Thiago T. Silva");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_aluno", 34);
                        //cmd.Parameters.AddWithValue("@nome", "MottaM M. MochtMorgen");
                        //cmd.Parameters.AddWithValue("@genero", "Masculino");
                        //cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                        //cmd.Parameters.AddWithValue("@telefone", "+244939212554");
                        //cmd.Parameters.AddWithValue("@turma_fk", 2);
                        //cmd.ExecuteNonQuery();
                        #endregion
                        #region TURMA_TB
                        //cmd.CommandText = "INSERT INTO turma_tb (id_turma,turma,curso,classe) " +
                        //"VALUES(@id_turma,@turma,@curso,@classe)";
                        //cmd.Parameters.AddWithValue("@id_turma", 1);
                        //cmd.Parameters.AddWithValue("@turma", "TI12TA");
                        //cmd.Parameters.AddWithValue("@curso", 1);
                        //cmd.Parameters.AddWithValue("@classe", "12ª");
                        //cmd.ExecuteNonQuery();

                        //cmd.Parameters.AddWithValue("@id_turma", 2);
                        //cmd.Parameters.AddWithValue("@turma", "TI11MA");
                        //cmd.Parameters.AddWithValue("@curso", 1);
                        //cmd.Parameters.AddWithValue("@classe", "12ª");
                        //cmd.ExecuteNonQuery();

                        #endregion

                    }
                    trans.Commit();
                };//fim beggim
                connect.Close();
                connect.Dispose();
            };
        }

        public void InsertOnUserTb(string path)
        {
            using (var conect = new SqliteConnection("Data source = " + path))
            {
                conect.Open();
                using (var cmd = new SqliteCommand(conect))
                {
                    cmd.CommandText =
                                   "INSERT INTO user_tb(id_user,telefone,senha,tipo_conta)" +
                                   "VALUES (@id_user,@telefone,@senha,@tipo_conta)";

                    cmd.Parameters.AddWithValue("@id_user", 1);
                    cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                    cmd.Parameters.AddWithValue("@senha", "pensador43");
                    cmd.Parameters.AddWithValue("@tipo_conta", "admin");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 2);
                    cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                    cmd.Parameters.AddWithValue("@senha", "leonel");
                    cmd.Parameters.AddWithValue("@tipo_conta", "admin");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 3);
                    cmd.Parameters.AddWithValue("@telefone", "+244911457117");
                    cmd.Parameters.AddWithValue("@senha", "manuel");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 4);
                    cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                    cmd.Parameters.AddWithValue("@senha", "vemba");
                    cmd.Parameters.AddWithValue("@tipo_conta", "admin");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 5);
                    cmd.Parameters.AddWithValue("@telefone", "+244931926436");
                    cmd.Parameters.AddWithValue("@senha", "ramiro");
                    cmd.Parameters.AddWithValue("@tipo_conta", "admin");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 6);
                    cmd.Parameters.AddWithValue("@telefone", "+244947682919");
                    cmd.Parameters.AddWithValue("@senha", "faustino");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 7);
                    cmd.Parameters.AddWithValue("@telefone", "+244939212554");
                    cmd.Parameters.AddWithValue("@senha", "bento");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 8);
                    cmd.Parameters.AddWithValue("@telefone", "+244992733226");
                    cmd.Parameters.AddWithValue("@senha", "lezadomingos");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 9);
                    cmd.Parameters.AddWithValue("@telefone", "+244912766637");
                    cmd.Parameters.AddWithValue("@senha", "mario");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 10);
                    cmd.Parameters.AddWithValue("@telefone", "+244912242750");
                    cmd.Parameters.AddWithValue("@senha", "costa");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 11);
                    cmd.Parameters.AddWithValue("@telefone", "+244939005616");
                    cmd.Parameters.AddWithValue("@senha", "silvina");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 12);
                    cmd.Parameters.AddWithValue("@telefone", "+244912447222");
                    cmd.Parameters.AddWithValue("@senha", "eduinarocha");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 13);
                    cmd.Parameters.AddWithValue("@telefone", "+244924117597");
                    cmd.Parameters.AddWithValue("@senha", "josemar");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 14);
                    cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                    cmd.Parameters.AddWithValue("@senha", "Abel");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 15);
                    cmd.Parameters.AddWithValue("@telefone", "+244993356447");
                    cmd.Parameters.AddWithValue("@senha", "luizviana12");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 16);
                    cmd.Parameters.AddWithValue("@telefone", "+244943187545");
                    cmd.Parameters.AddWithValue("@senha", "simao65");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 17);
                    cmd.Parameters.AddWithValue("@telefone", "+244997219116");
                    cmd.Parameters.AddWithValue("@senha", "victorinomussoque");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_user", 18);
                    cmd.Parameters.AddWithValue("@telefone", "+244992851726");
                    cmd.Parameters.AddWithValue("@senha", "cerca");
                    cmd.Parameters.AddWithValue("@tipo_conta", "prof");
                    cmd.ExecuteNonQuery();
                };
                conect.Close();
                conect.Dispose();

            };

        }

        public void InsertOnDireitorTB(string path)
        {
            using (var conect = new SqliteConnection("Data source = " + path))
            {
                conect.Open();
                using (var cmd = new SqliteCommand(conect))
                {
                    cmd.CommandText =
                           "INSERT INTO direitor_tb(id_direitor, nome, telefone, nascimento, user_fk)" +
                           "VALUES(@id_direitor,@nome,@telefone,@nascimento,@user_fk)";

                    cmd.Parameters.AddWithValue("@id_direitor", 1);
                    cmd.Parameters.AddWithValue("@nome", "Santos Ferreira Campos");
                    cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                    cmd.Parameters.AddWithValue("@nascimento", "1996-12-18");
                    cmd.Parameters.AddWithValue("@user_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_direitor", 2);
                    cmd.Parameters.AddWithValue("@nome", "Angelo Leonel da Rocha");
                    cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                    cmd.Parameters.AddWithValue("@nascimento", "1998-19-02");
                    cmd.Parameters.AddWithValue("@user_fk",2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_direitor", 3);
                    cmd.Parameters.AddWithValue("@nome", "Malaquias Vemba");
                    cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                    cmd.Parameters.AddWithValue("@nascimento", "2000-18-21");
                    cmd.Parameters.AddWithValue("@user_fk", 4);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_direitor", 4);
                    cmd.Parameters.AddWithValue("@nome", "Ramiro Cardoso");
                    cmd.Parameters.AddWithValue("@telefone", "+244931926436");
                    cmd.Parameters.AddWithValue("@nascimento", "1985-21-04");
                    cmd.Parameters.AddWithValue("@user_fk", 5);
                    cmd.ExecuteNonQuery();
                };
                conect.Close();
                conect.Dispose();

            };

        }
        
        public void InsertOnCursoTB(string path)
        {
            using (var conect = new SqliteConnection("Data source = " + path))
            {
                conect.Open();
                using (var cmd = new SqliteCommand(conect))
                {
                    cmd.CommandText =
                       "INSERT INTO curso_tb(id_curso,curso, direitor_fk)" +
                       "VALUES(@id_curso,@curso,@direitor_fk)";

                    cmd.Parameters.AddWithValue("@id_curso", 1);
                    cmd.Parameters.AddWithValue("@curso", "Tecnico de Informatica");
                    cmd.Parameters.AddWithValue("@direitor_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_curso", 2);
                    cmd.Parameters.AddWithValue("@curso", "Desenhador Projetista");
                    cmd.Parameters.AddWithValue("@direitor_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_curso", 3);
                    cmd.Parameters.AddWithValue("@curso", "Contabilidade");
                    cmd.Parameters.AddWithValue("@direitor_fk", 3);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_curso", 4);
                    cmd.Parameters.AddWithValue("@curso", "Informatica de gestao");
                    cmd.Parameters.AddWithValue("@direitor_fk", 5);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_curso", 5);
                    cmd.Parameters.AddWithValue("@curso", "Contruicao civil");
                    cmd.Parameters.AddWithValue("@direitor_fk", 4);
                    cmd.ExecuteNonQuery();
                };
                conect.Close();
                conect.Dispose();

            };

        }

        public void InsertOnTurmaTB(string path)
                {
                    using (var conect = new SqliteConnection("Data source = " + path))
                    {
                        conect.Open();
                        using (var cmd = new SqliteCommand(conect))
                        {
                            cmd.CommandText = "INSERT INTO turma_tb (id_turma,turma,curso_fk,classe) " +
                                  "VALUES(@id_turma,@turma,@curso_fk,@classe)";
                            cmd.Parameters.AddWithValue("@id_turma", 1);
                            cmd.Parameters.AddWithValue("@turma", "TI12TA");
                            cmd.Parameters.AddWithValue("@curso_fk", 1);
                            cmd.Parameters.AddWithValue("@classe", "12ª");
                           
                            cmd.ExecuteNonQuery();

                            cmd.Parameters.AddWithValue("@id_turma", 2);
                            cmd.Parameters.AddWithValue("@turma", "TI11MA");
                            cmd.Parameters.AddWithValue("@curso_fk", 1);
                            cmd.Parameters.AddWithValue("@classe", "11ª");
                            
                            cmd.ExecuteNonQuery();
                        };
                        conect.Close();
                        conect.Dispose();

                    };

                }

        public void InsertOnAlunoTB(string path)
        {
            using (var conect = new SqliteConnection("Data source = " + path))
            {
                conect.Open();
                using (var cmd = new SqliteCommand(conect))
                {
                    cmd.CommandText = "INSERT INTO aluno_tb (id_aluno,nome,genero,nascimento,telefone,turma_fk) " +
                                                "VALUES(@id_aluno,@nome,@genero,@nascimento,@telefone,@turma_fk)";


                    //TURMA TI12MA-----------------------------------------------------------1
                    cmd.Parameters.AddWithValue("@id_aluno", 1);
                    cmd.Parameters.AddWithValue("@nome", "Root danke");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244947682919");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 2);
                    cmd.Parameters.AddWithValue("@nome", "Jessienh haus");
                    cmd.Parameters.AddWithValue("@genero", "Feminino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244939212554");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 3);
                    cmd.Parameters.AddWithValue("@nome", "Analia de Jesus");
                    cmd.Parameters.AddWithValue("@genero", "Feminino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 4);
                    cmd.Parameters.AddWithValue("@nome", "Sechs Nacht");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244912766637");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 5);
                    cmd.Parameters.AddWithValue("@nome", "Iron Morgen");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244992733226");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 6);
                    cmd.Parameters.AddWithValue("@nome", "Bitte Slecht");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244912442750");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 7);
                    cmd.Parameters.AddWithValue("@nome", "Cehna Wiedersehn");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244939005616");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 8);
                    cmd.Parameters.AddWithValue("@nome", "Adam Smith");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244912447222");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 9);
                    cmd.Parameters.AddWithValue("@nome", "Alan keymore");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244924117597");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 10);
                    cmd.Parameters.AddWithValue("@nome", "Monich via");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 11);
                    cmd.Parameters.AddWithValue("@nome", "Ramiro Tschss");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244911457117");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 12);
                    cmd.Parameters.AddWithValue("@nome", "Jessica da Costa");
                    cmd.Parameters.AddWithValue("@genero", "Feminino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 13);
                    cmd.Parameters.AddWithValue("@nome", "Roberta Miranda");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 14);
                    cmd.Parameters.AddWithValue("@nome", "Gray NickMcbone");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244931926436");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 15);
                    cmd.Parameters.AddWithValue("@nome", "Backbone Mcmc");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244992733226");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 16);
                    cmd.Parameters.AddWithValue("@nome", "Thiago Silva");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+224912242750");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 17);
                    cmd.Parameters.AddWithValue("@nome", "Motta MochtMorgen");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.ExecuteNonQuery();

                    //TURMA TI10MA-----------------------------------------------------------2
                    cmd.Parameters.AddWithValue("@id_aluno", 18);
                    cmd.Parameters.AddWithValue("@nome", "Roobert Kehn");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 19);
                    cmd.Parameters.AddWithValue("@nome", "Jessey Adam");
                    cmd.Parameters.AddWithValue("@genero", "Feminino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 20);
                    cmd.Parameters.AddWithValue("@nome", "Josemar cristovão");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 21);
                    cmd.Parameters.AddWithValue("@nome", "Bossac Mcmore");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244991669525");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 22);
                    cmd.Parameters.AddWithValue("@nome", "Ihr Sehr Mag");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 23);
                    cmd.Parameters.AddWithValue("@nome", "Mutter sind");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 24);
                    cmd.Parameters.AddWithValue("@nome", "Wiedersehn Haben");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 25);
                    cmd.Parameters.AddWithValue("@nome", "Adam Has stress");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244992733224");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 26);
                    cmd.Parameters.AddWithValue("@nome", "Domingos Armando");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 27);
                    cmd.Parameters.AddWithValue("@nome", "Valeria Aynna");
                    cmd.Parameters.AddWithValue("@genero", "Femenino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 28);
                    cmd.Parameters.AddWithValue("@nome", "Roobertt Tschss");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                    cmd.Parameters.AddWithValue("@turma_fk", "+244927515922");
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 29);
                    cmd.Parameters.AddWithValue("@nome", "Jessica da Costa");
                    cmd.Parameters.AddWithValue("@genero", "Feminino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244927515922");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 30);
                    cmd.Parameters.AddWithValue("@nome", "Roberta Straass Miranda");
                    cmd.Parameters.AddWithValue("@genero", "Femenino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 31);
                    cmd.Parameters.AddWithValue("@nome", "Gray Haus NickMcbone");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 32);
                    cmd.Parameters.AddWithValue("@nome", "Backbone Mc. Mcmc");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 33);
                    cmd.Parameters.AddWithValue("@nome", "Thiago T. Silva");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244930249537");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_aluno", 34);
                    cmd.Parameters.AddWithValue("@nome", "MottaM M. MochtMorgen");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@telefone", "+244939212554");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.ExecuteNonQuery();
                };
                conect.Close();
                conect.Dispose();

            };

        }
        
        public void InsertOnDisciplinaTB(string path)
        {
            using (var conect = new SqliteConnection("Data source = " + path))
            {
                conect.Open();
                using (var cmd = new SqliteCommand(conect))
                {
                    cmd.CommandText =
                           "INSERT INTO disciplina_tb (id_disciplina,disciplina,direitor_fk,curso_fk)" +
                           "VALUES (@id_disciplina,@disciplina,@direitor_fk,@curso_fk)";

                    cmd.Parameters.AddWithValue("@id_disciplina", 1);
                    cmd.Parameters.AddWithValue("@disciplina", "Matemática");
                    cmd.Parameters.AddWithValue("@direitor_fk", 1);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 2);
                    cmd.Parameters.AddWithValue("@disciplina", "TLP");
                    cmd.Parameters.AddWithValue("@direitor_fk", 2);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 3);
                    cmd.Parameters.AddWithValue("@disciplina", "TREI");
                    cmd.Parameters.AddWithValue("@direitor_fk", 4);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 4);
                    cmd.Parameters.AddWithValue("@disciplina", "Física");
                    cmd.Parameters.AddWithValue("@direitor_fk", 3);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 5);
                    cmd.Parameters.AddWithValue("@disciplina", "SEAC");
                    cmd.Parameters.AddWithValue("@direitor_fk", 2);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 6);
                    cmd.Parameters.AddWithValue("@disciplina", "PT");
                    cmd.Parameters.AddWithValue("@direitor_fk", 3);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();
                    //fim tarde

                    //inicio Manhã
                    cmd.Parameters.AddWithValue("@id_disciplina", 7);
                    cmd.Parameters.AddWithValue("@disciplina", "Matemática");
                    cmd.Parameters.AddWithValue("@direitor_fk", 2);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 8);
                    cmd.Parameters.AddWithValue("@disciplina", "Física");
                    cmd.Parameters.AddWithValue("@direitor_fk", 4);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 9);
                    cmd.Parameters.AddWithValue("@disciplina", "Lingua Portuguesa");
                    cmd.Parameters.AddWithValue("@direitor_fk", 1);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 10);
                    cmd.Parameters.AddWithValue("@disciplina", "TLP");
                    cmd.Parameters.AddWithValue("@direitor_fk", 2);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 11);
                    cmd.Parameters.AddWithValue("@disciplina", "TIC");
                    cmd.Parameters.AddWithValue("@direitor_fk", 3);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 12);
                    cmd.Parameters.AddWithValue("@disciplina", "SEAC");
                    cmd.Parameters.AddWithValue("@direitor_fk", 1);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 13);
                    cmd.Parameters.AddWithValue("@disciplina", "Língua Inlgêsa");
                    cmd.Parameters.AddWithValue("@direitor_fk", 2);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 14);
                    cmd.Parameters.AddWithValue("@disciplina", "FAI");
                    cmd.Parameters.AddWithValue("@direitor_fk", 3);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 15);
                    cmd.Parameters.AddWithValue("@disciplina", "EDfísica");
                    cmd.Parameters.AddWithValue("@direitor_fk", 3);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 16);
                    cmd.Parameters.AddWithValue("@disciplina", "Electrotecnia");
                    cmd.Parameters.AddWithValue("@direitor_fk", 4);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_disciplina", 17);
                    cmd.Parameters.AddWithValue("@disciplina", "OGI");
                    cmd.Parameters.AddWithValue("@direitor_fk", 2);
                    cmd.Parameters.AddWithValue("@curso_fk", 1);
                    cmd.ExecuteNonQuery();

                };
                conect.Close();
                conect.Dispose();

            };

        }

        public void InsertOnProfessorTB(string path)
        {
            using (var conect = new SqliteConnection("Data source = " + path))
            {
                conect.Open();
                using (var cmd = new SqliteCommand(conect))
                {
                    cmd.CommandText = "INSERT INTO professor_tb (id_prof,nome,genero,salario,nascimento,disciplina_fk,user_fk ) " +
                           "VALUES(@id_prof,@nome,@genero,@salario,@nascimento,@disciplina_fk,@user_fk)";
                    //professore do turno = TARDE
                    cmd.Parameters.AddWithValue("@id_prof", 1);
                    cmd.Parameters.AddWithValue("@nome", "Santos Ferreira Campos");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 800530.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 1);//Math
                    cmd.Parameters.AddWithValue("@user_fk", 1);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 2);
                    cmd.Parameters.AddWithValue("@nome", "Emanuel Ngola");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 100330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 2);//TLP
                    cmd.Parameters.AddWithValue("@user_fk", 3);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 3);
                    cmd.Parameters.AddWithValue("@nome", "Ramiro Cardoso");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 200330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 3);//TREI
                    cmd.Parameters.AddWithValue("@user_fk", 5);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 4);
                    cmd.Parameters.AddWithValue("@nome", "Angelo Leonel Da costa");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 304230.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 4);//Física
                    cmd.Parameters.AddWithValue("@user_fk", 2);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 5);
                    cmd.Parameters.AddWithValue("@nome", "Bento Pedro");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 500330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 5);//SEAC
                    cmd.Parameters.AddWithValue("@user_fk", 7);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 6);
                    cmd.Parameters.AddWithValue("@nome", "Faustino Samaiavo");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 100330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 6);//PT
                    cmd.Parameters.AddWithValue("@user_fk", 6);
                    cmd.ExecuteNonQuery();
                    //professore do turno = TARDE

                    //professore do turno = Manhã
                    cmd.Parameters.AddWithValue("@id_prof", 7);
                    cmd.Parameters.AddWithValue("@nome", "Domingos Leza ");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 600230.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 17);//OGI
                    cmd.Parameters.AddWithValue("@user_fk", 8);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 8);
                    cmd.Parameters.AddWithValue("@nome", "Mario Emilio");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 602330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 7);//MAth-manhã
                    cmd.Parameters.AddWithValue("@user_fk", 9);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 9);
                    cmd.Parameters.AddWithValue("@nome", "António da Costa");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 102330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 8);//Física-manhã
                    cmd.Parameters.AddWithValue("@user_fk", 10);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 10);
                    cmd.Parameters.AddWithValue("@nome", "Silvína C. Silvina");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 302330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk",9);//L.port-manhã
                    cmd.Parameters.AddWithValue("@user_fk", 11);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 11);
                    cmd.Parameters.AddWithValue("@nome", "Eduina da Rocha");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 230330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk",15);//EDFISICA-Manhã
                    cmd.Parameters.AddWithValue("@user_fk", 12);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 12);
                    cmd.Parameters.AddWithValue("@nome", "José de Jesus Pedro");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 230230.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 11);//TIC-Manhã
                    cmd.Parameters.AddWithValue("@user_fk", 13);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 13);
                    cmd.Parameters.AddWithValue("@nome", "Eduardo Abel");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 120330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 12);//SEAC-Manhã
                    cmd.Parameters.AddWithValue("@user_fk", 14);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 14);
                    cmd.Parameters.AddWithValue("@nome", "Luiz Viana");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 400330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 13);//ING-Manhã
                    cmd.Parameters.AddWithValue("@user_fk", 15);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 15);
                    cmd.Parameters.AddWithValue("@nome", "Simao Diogo");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 300330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 14);//FAI-Manhã
                    cmd.Parameters.AddWithValue("@user_fk", 16);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 16);
                    cmd.Parameters.AddWithValue("@nome", "Luiz Viana");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 703330.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 16);//ELECTROTECNIA-Manhã
                    cmd.Parameters.AddWithValue("@user_fk", 15);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_prof", 17);
                    cmd.Parameters.AddWithValue("@nome", "Victor Mussoque");
                    cmd.Parameters.AddWithValue("@genero", "Masculino");
                    cmd.Parameters.AddWithValue("@salario", 230.000);
                    cmd.Parameters.AddWithValue("@nascimento", "1987/03/09");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 10);//TLP-Manhã
                    cmd.Parameters.AddWithValue("@user_fk", 17);
                    cmd.ExecuteNonQuery();
                    //professore do turno = MANhã
                };
                conect.Close();
                conect.Dispose();

            };

        }

        public void InsertOnHorario12TA(string path)
        {
            using (var conect = new SqliteConnection("Data source = " + path))
            {
                conect.Open();
                using (var cmd = new SqliteCommand(conect))
                {
                    cmd.CommandText =
                        "INSERT INTO horario_tb (id_horario,hora_inicio,hora_fim,dia,turma_fk,periodo,disciplina_fk) " +
                        "VALUES(@id_horario,@hora_inicio,@hora_fim,@dia,@turma_fk,@periodo,@disciplina_fk)";


                    //segunda
                    cmd.Parameters.AddWithValue("@id_horario", 1);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 2);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 5);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 3);
                    cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 4);
                    cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                    cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 5);
                    cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                    cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 6);
                    cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                    cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                    cmd.ExecuteNonQuery();
                    //fiim segunda

                    //terça-feora
                    cmd.Parameters.AddWithValue("@id_horario", 7);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 17);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 8);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 17);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 9);
                    cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 10);
                    cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                    cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 6);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 11);
                    cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                    cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 12);
                    cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                    cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                    cmd.ExecuteNonQuery();

                    //fim terça feira

                    //quarta-feira inicio

                    cmd.Parameters.AddWithValue("@id_horario", 13);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 4);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 14);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 4);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 15);
                    cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 16);
                    cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                    cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 17);
                    cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                    cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 18);
                    cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                    cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 1);
                    cmd.ExecuteNonQuery();
                    //fim quarta feira

                    //quinta feira

                    cmd.Parameters.AddWithValue("@id_horario", 19);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 4);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 20);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 4);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 21);
                    cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 22);
                    cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                    cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 23);
                    cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                    cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 24);
                    cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                    cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                    cmd.ExecuteNonQuery();

                    //fim quinta feira

                    //sexta feira

                    cmd.Parameters.AddWithValue("@id_horario", 25);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "13:50");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 26);
                    cmd.Parameters.AddWithValue("@hora_inicio", "13:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "14:45");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 2);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 27);
                    cmd.Parameters.AddWithValue("@hora_inicio", "14:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "15:40");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 28);
                    cmd.Parameters.AddWithValue("@hora_inicio", "15:45");
                    cmd.Parameters.AddWithValue("@hora_fim", "16:35");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 3);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 29);
                    cmd.Parameters.AddWithValue("@hora_inicio", "16:40");
                    cmd.Parameters.AddWithValue("@hora_fim", "17:30");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 5);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 30);
                    cmd.Parameters.AddWithValue("@hora_inicio", "17:35");
                    cmd.Parameters.AddWithValue("@hora_fim", "18:25");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 1);
                    cmd.Parameters.AddWithValue("@periodo", "Tarde");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 5);
                    cmd.ExecuteNonQuery();

                    //fim sexta feira
                };
                conect.Close();
                conect.Dispose();

            };

        }

        public void InsertOnHorario11MA(string path)
        {
            using (var conect = new SqliteConnection("Data source = " + path))
            {
                conect.Open();
                using (var cmd = new SqliteCommand(conect))
                {
                    //_____________________________________Manhã_______________________________________________________


                    cmd.CommandText =
                        "INSERT INTO horario_tb (id_horario,hora_inicio,hora_fim,dia,turma_fk,periodo,disciplina_fk) " +
                        "VALUES(@id_horario,@hora_inicio,@hora_fim,@dia,@turma_fk,@periodo,@disciplina_fk)";

                    //segunda
                    cmd.Parameters.AddWithValue("@id_horario", 31);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",12);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 32);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",12);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 33);
                    cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 13);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 34);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 13);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 35);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 7);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 36);
                    cmd.Parameters.AddWithValue("@hora_inicio", "11:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "12:40");
                    cmd.Parameters.AddWithValue("@dia", "segunda-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 7);
                    cmd.ExecuteNonQuery();
                    //fiim segunda

                    //terça-feora
                    cmd.Parameters.AddWithValue("@id_horario", 37);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 14);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 38);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 14);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 39);
                    cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",8);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 40);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 8);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 41);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",10);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 42);
                    cmd.Parameters.AddWithValue("@hora_inicio", "11:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "12:40");
                    cmd.Parameters.AddWithValue("@dia", "terça-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",10);
                    cmd.ExecuteNonQuery();

                    //fim terça feira

                    //quarta-feira inicio
                    cmd.Parameters.AddWithValue("@id_horario", 43);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",9);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 44);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",9);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 45);
                    cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk",2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",8);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 46);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@di sciplina_fk",13);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 47);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",10);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 48);
                    cmd.Parameters.AddWithValue("@hora_inicio", "11:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "12:40");
                    cmd.Parameters.AddWithValue("@dia", "quarta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",10);
                    cmd.ExecuteNonQuery();
                    //fim quarta feira

                    //quinta feira
                    cmd.Parameters.AddWithValue("@id_horario", 49);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 12);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 50);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk",9);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 51);
                    cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 11);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 52);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 11);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 53);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 10);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 54);
                    cmd.Parameters.AddWithValue("@hora_inicio", "11:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "12:40");
                    cmd.Parameters.AddWithValue("@dia", "quinta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 10);
                    cmd.ExecuteNonQuery();

                    //fim quinta feira

                    //sexta feira
                    cmd.Parameters.AddWithValue("@id_horario", 55);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "07:50");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 11);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 56);
                    cmd.Parameters.AddWithValue("@hora_inicio", "07:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "08:45");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 11);
                    cmd.ExecuteNonQuery();



                    cmd.Parameters.AddWithValue("@id_horario", 57);
                    cmd.Parameters.AddWithValue("@hora_inicio", "08:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "09:40");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 7);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 58);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:00");
                    cmd.Parameters.AddWithValue("@hora_fim", "10:50");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 7);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 59);
                    cmd.Parameters.AddWithValue("@hora_inicio", "10:55");
                    cmd.Parameters.AddWithValue("@hora_fim", "11:45");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 16);
                    cmd.ExecuteNonQuery();


                    cmd.Parameters.AddWithValue("@id_horario", 60);
                    cmd.Parameters.AddWithValue("@hora_inicio", "11:50");
                    cmd.Parameters.AddWithValue("@hora_fim", "12:40");
                    cmd.Parameters.AddWithValue("@dia", "sexta-feira");
                    cmd.Parameters.AddWithValue("@turma_fk", 2);
                    cmd.Parameters.AddWithValue("@periodo", "Manhã");
                    cmd.Parameters.AddWithValue("@disciplina_fk", 16);
                    cmd.ExecuteNonQuery();
                };
                conect.Close();
                conect.Dispose();

            };

        }

        #endregion ATSplashClassEnd

    }
}

