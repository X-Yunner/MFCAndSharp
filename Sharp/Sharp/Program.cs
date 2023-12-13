using System;
using System.IO;
using System.Windows.Forms;
using Vanara.PInvoke;

namespace Sharp
{
    public enum WM : uint
    {
        WM_USER = User32.WM_USER,//0x0400,
        WM_GET_HANDLE = WM_USER + 1, //传送hwnd消息
        WM_NEED_CLOSE = WM_USER + 2, //程序需要关闭消息 调用方 向被 调用方 发送
        WM_ALERADY_EXIT = WM_USER + 3, //程序退出消息 被调用方 向调用方 发送
        WM_HAVE_ERROR = WM_USER + 4, //程序运行异常消息 被调用方 向调用方 发送
    }

    public class Program
    {
        static void Main(string[] args)
        {
            HWND TransferHwnd;
            if (args.Length != 2 || args[0] != "-h") return;
            int hwnd;
            if (!int.TryParse(args[1], out hwnd)) return;
            TransferHwnd = new HWND(new IntPtr(hwnd));
            if (TransferHwnd == HWND.NULL) return;
            
            Win32MessagerHandleForm win32MessagerHandle = new Win32MessagerHandleForm();
            win32MessagerHandle.TransferHwnd = TransferHwnd;
            Application.Run(win32MessagerHandle);


        }

    }

    public class Win32MessagerHandleForm : Form
    {
        public string CurrentPath;

        public HWND TransferHwnd;

        public Win32MessagerHandleForm()
        {
            CurrentPath = AppDomain.CurrentDomain.BaseDirectory;
            Visible = false;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            User32.PostMessage(TransferHwnd, WM.WM_GET_HANDLE, Handle);
            User32.PostMessage(TransferHwnd, WM.WM_HAVE_ERROR);
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            User32.PostMessage(TransferHwnd, WM.WM_ALERADY_EXIT);
            base.OnClosed(e);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WM.WM_NEED_CLOSE:
                {
                    Close();
                    break;
                }
                case (int)WM.WM_HAVE_ERROR:
                {
                    File.WriteAllText(Path.Combine(CurrentPath, "error.txt"), m.WParam.ToString());
                    break;
                }
                default:
                {
                    base.WndProc(ref m);
                    break;
                }
            }
        }


    }

}
