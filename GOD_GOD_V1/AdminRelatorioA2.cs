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
    class AdminRelatorioA2 : BaseAdapter<string>
    {
        private Context meuContexto;
        private List<string> __mPROF__,__mALUNOS__,__mSUMARIO__;
        


        public AdminRelatorioA2(Context contexto, List<string>__PROF__, List<string> __ALUNOS__, List<string> __SUMARIO__)
        {
            meuContexto = contexto;
            __PROF__= __PROF__;
            __mALUNOS__ = __ALUNOS__;
            __mSUMARIO__ = __mSUMARIO__;
        }

        public override int Count
        {
            get { return __mPROF__.Count; }
        }
        public override string this[int position]
        {
            get { return __mPROF__[position]; }
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
                    Resource.Layout.AdminListItemRelatorioA2,null,false);
            }
            var textoProf2 = line.FindViewById<TextView>(Resource.Id.prof___2);
            var textoaluno2 = line.FindViewById<TextView>(Resource.Id.aluno__2);
            var textoSumario2 = line.FindViewById<TextView>(Resource.Id.sumario__2);

            textoProf2.Text = __mPROF__[position];
            textoaluno2.Text =__mALUNOS__[position];
            textoSumario2.Text =__mSUMARIO__[position];


            return line;
        }
    }
}