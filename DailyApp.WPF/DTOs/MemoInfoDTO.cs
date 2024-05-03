using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.DTOs
{
    class MemoInfoDTO
    {
        private int waitID;
        private string title;
        private string content;
        private int status;
        public int WaitID
        {
            get { return waitID; }
            set { waitID = value; }
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

        public int Status // 状态
        {
            get { return status; }
            set { status = value; }
        }
    }
}
