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

namespace GOD_GOD_V1
{
    class AdminRelatorio : BaseAdapter<string>
    {
        private Context meuContexto;
        private List<string> mProf,mAlunos,mSumario;


        public AdminRelatorio(Context contexto, List<string>nomes, List<string> alunos, List<string> fsum)
        {
            meuContexto = contexto;
            mProf = nomes;
            mAlunos = alunos;
            mSumario = fsum;
        }

        public override int Count
        {
            get { return mProf.Count; }
        }
        public override string this[int position]
        {
            get { return mProf[position]; }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View line;
            line = convertView;
            if(line == null)
            {

                line = LayoutInflater.FromContext(meuContexto).Inflate(
                    Resource.Layout.AdminListItemRelatorioA1,null,false);
            }
            var textoProf = line.FindViewById<TextView>(Resource.Id.prof___);
            var textoaluno = line.FindViewById<TextView>(Resource.Id.aluno__);
            var textoSumario= line.FindViewById<TextView>(Resource.Id.sumario__);
            textoProf.Text =mProf[position];
            textoaluno.Text = mAlunos[position];
            textoSumario.Text = mSumario[position];



            return line;
        }
    }
}