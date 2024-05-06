using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.DTOs
{
    class MemoInfoDTO
    {
        private int memoID;
        private string title;
        private string content;
        public int MemoID
        {
            get { return memoID; }
            set { memoID = value; }
        }

        public string Title //标题
        {
            get { return title; }
            set { title = value; }
        }

        public string Content // 内容
        {
            get { return content; }
            set { content = value; }
        }
    }
}
