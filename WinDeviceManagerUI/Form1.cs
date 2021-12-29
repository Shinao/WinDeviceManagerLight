using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HardwareHelperLib;
using HH_Lib_Test;

/*
 * HW_Lib_Test
 * ===================================================
 * Simple Win Form to Demonstrate HH Lib
 * Windows XP SP2, VS2005 C#.NET, DotNet 2.0
 * ====================================================
 * LOG:      Who?    When?       What?
 * (v)1.0.0  WJF     11/26/07    Original Implementation
 */

namespace HW_Lib_Test
{
    public partial class Form1 : Form
    {
        //Global library declared
        HH_Lib hwh = new HH_Lib();
        List<DEVICE_INFO> HardwareList;

        //Name:     Form1()
        //Inputs:   none
        //Outputs:  none
        //Remarks:  Default constructor
        public Form1()
        {
            InitializeComponent();
        }
        public void ReloadHardwareList()
        {
            HardwareList = hwh.GetAll();
            listdevices.Items.Clear();
            foreach (var device in HardwareList)
            {
                ListViewItem lvi = new ListViewItem(new string[] { device.name, device.friendlyName, device.hardwareId, device.status.ToString() });
                listdevices.Items.Add(lvi);
            }
            label1.Text = HardwareList.Count.ToString() + " Devices Attached";
        }
        //Name:     Form1_Load()
        //Inputs:   object, eventArgs
        //Outputs:  none
        //Remarks:  In the form load we take an initial hardware inventroy,
        //          then hook the notifications so we can respond if any
        //          device is added or removed.
        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.searchTextBox;

            ReloadHardwareList();

            hwh.HookHardwareNotifications(this.Handle, true);
        }
        //Name:     Form1_FormClosing
        //Inputs:   object, eventArgs
        //Outputs:  none
        //Remarks:  Whenever the form closes we need to unregister the
        //          hardware notifier.  Failure to do so could cause
        //          the system not to release some resources.  Calling
        //          this method if you are not currently hooking the
        //          hardware events has no ill effects so better to be
        //          safe than sorry.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            hwh.CutLooseHardwareNotifications(this.Handle);
            hwh = null;
        }
        //Name:     WndProc
        //Inputs:   Message
        //Outputs:  none
        //Remarks:  This is the override for the window message handler.  Here
        //          is where we can respond to our DEVICECHANGE message we are
        //          hooking.  If we received a hardware change notification 
        //          the method reloads our hardware list.  Otherwise, it must
        //          call the default handler so the message can be processed.
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case HardwareHelperLib.Native.WM_DEVICECHANGE:
                    {
                        if (m.WParam.ToInt32() == HardwareHelperLib.Native.DBT_DEVNODES_CHANGED)
                        {
                            ReloadHardwareList();
                        }
                        break;
                    }
            }
            base.WndProc(ref m);
        }
        //Name:     button1_Clck
        //Inputs:   object, eventArgs
        //Outputs:  none
        //Remarks:  We are using this button to demonstrate enabling a
        //          hardware device.  There are several things worth
        //          noting.  First, just to be safe we are disabling
        //          hardware notifcations until we are done.  The UI
        //          thread in .NET won't let the WndProc method run
        //          to my knowledge while you are in here but if you 
        //          were invoking these methods on different callers it
        //          would be worthwhile to disable the notifications
        //          during.  The call to SetDeviceState is designed 
        //          to allow you to pass in multiple devices in an
        //          array to disable, even though we are currently just
        //          doing the selected one.  Also the search is a
        //          substring search so be careful not to use something
        //          so generic that it will affect more devices than
        //          the one(s) you intended.  See the notes for the
        //          SetDeviceState method in the library for some
        //          important info.
        private void button1_Click(object sender, EventArgs e)
        {
            if (listdevices.SelectedIndices.Count == 0)
                return;

            hwh.CutLooseHardwareNotifications(this.Handle);
            try
            {
                hwh.SetDeviceState(HardwareList[listdevices.SelectedIndices[0]], true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
            hwh.HookHardwareNotifications(this.Handle, true);
        }
        //Name:     button2_Clck
        //Inputs:   object, eventArgs
        //Outputs:  none
        //Remarks:  We are using this button to disable a device.
        //          See remarks above.
        private void button2_Click(object sender, EventArgs e)
        {
            if (listdevices.SelectedIndices.Count == 0)
                return;

            hwh.CutLooseHardwareNotifications(this.Handle);

            try
            {
                hwh.SetDeviceState(HardwareList[listdevices.SelectedIndices[0]], false);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }

            hwh.HookHardwareNotifications(this.Handle, true);
        }
    }
}