using System.Collections.Generic;
using System.Linq;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Models
{
    public class Shared
    {
        QuanLyDienThoaiDataContext db = new QuanLyDienThoaiDataContext();
        public string AddCommas(int pNumber)
        {
            return string.Format("{0:N0}", pNumber) + "đ";
        }

        public string AddCommas_Long(long pNumber)
        {
            return string.Format("{0:N0}", pNumber) + "đ";
        }
        public string DeleteCommas(string pStr)
        {
            pStr = pStr.Replace('đ', ' ');
            pStr = pStr.Trim();
            string newStr = "";
            string[] list = pStr.Split(new char[] { ',', '.' });
            foreach (string str in list)
            {
                newStr += str;
            }

            return newStr;
        }
        public string Add3Zero(string pStr)
        {
            int number = int.Parse(pStr) + 1;
            string newStr = number.ToString();
            if (number >= 1 && number <= 999)
            {
                while (newStr.Length < 4)
                {
                    newStr = newStr.Insert(0, "0");
                }
            }
            return newStr;
        }

        public string CreateMaHoaDon()
        {
            var list = db.PHIEUXUATs.ToList();
            int max = 0;
            foreach (var item in list)
            {
                int tam = int.Parse(item.PX_MA.Substring(2, 4));
                if (tam > max)
                {
                    max = tam;
                }
            }
            max++;
            string ma = max.ToString();
            while (ma.Length < 4)
            {
                ma = "0" + ma;
            }

            return "PX" + ma;
        }

        public string CreateMaPhieuNhap()
        {
            var list = db.PHIEUNHAPs.ToList();
            int max = 0;
            foreach (var item in list)
            {
                int tam = int.Parse(item.PN_MA.Substring(2, 4));
                if (tam > max)
                {
                    max = tam;
                }
            }
            max++;
            string ma = max.ToString();
            while (ma.Length < 4)
            {
                ma = "0" + ma;
            }

            return "PN" + ma;
        }

        public List<List<QuanLyDienThoai>> ProductCarousel(List<QuanLyDienThoai> list, int count)
        {
            List<List<QuanLyDienThoai>> newList = new List<List<QuanLyDienThoai>>();
            int startIndex = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    if (i % count == 0)
                    {
                        newList.Add(list.GetRange(startIndex, count));

                        startIndex = i;
                    }
                }
            }

            newList.Add(list.Skip(newList.Count * count).ToList());

            return newList;
        }
    }
}