using System;
using System.ComponentModel;
using System.Globalization;
using DevExpress.Data.Mask;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;


namespace Tiveria.Common.Controls
{
    public class IPAddressEdit : TimeEdit
    {
        static IPAddressEdit()
        {
            RepositoryItemIPAddressEdit.Register();
        }
        public IPAddressEdit()
            : base()
        {
            this.fOldEditValue = this.fEditValue = new DateTime(0);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemIPAddressEdit Properties
        {
            get
            {
                return base.Properties as RepositoryItemIPAddressEdit;
            }
        }
        public override string EditorTypeName
        {
            get
            {
                return RepositoryItemIPAddressEdit.IPAddressEditName;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override object EditValue
        {
            get
            {
                if (Properties.ExportMode == ExportMode.DisplayText)
                    return Properties.GetDisplayText(null, base.EditValue);

                if (base.EditValue is DateTime && IPAddressHelper.IsConvertible((DateTime)base.EditValue))
                    return IPAddressHelper.ToIPAddress((DateTime)base.EditValue);

                if (base.EditValue is IPv4Addr)
                    return base.EditValue;

                if (base.EditValue is string)
                    return new IPv4Addr((string)base.EditValue);

                return new IPv4Addr("0.0.0.0");
            }
            set
            {
                if (value is IPv4Addr)
                    base.EditValue = value;
                else if (value is string && (string)value != "")
                    base.EditValue = new IPv4Addr((string)value);
                else if (value is DateTime && IPAddressHelper.IsConvertible((DateTime)value))
                    base.EditValue = IPAddressHelper.ToIPAddress((DateTime)value);
                else
                    base.EditValue = new IPv4Addr("0.0.0.0");
            }
        }
        [Browsable(false)]
        public new DateTime Time
        {
            get
            {
                return base.Time;
            }
            set
            {
                base.Time = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string IPAddress
        {
            get
            {
                return EditValue.ToString();
            }
            set
            {
                EditValue = value;
            }
        }

        protected override MaskManager CreateMaskManager(MaskProperties mask)
        {
            IPAddressEditMaskProperties patchedMask = new IPAddressEditMaskProperties();
            patchedMask.Assign(mask);
            patchedMask.EditMask = Properties.GetFormatMaskAccessFunction(mask.EditMask, mask.Culture);

            return patchedMask.CreatePatchedMaskManager();
        }
    }

    public class RepositoryItemIPAddressEdit : RepositoryItemTimeEdit
    {
        internal const string IPAddressEditName = "IPAddressEdit";

        static RepositoryItemIPAddressEdit()
        {
            Register();
        }
        public RepositoryItemIPAddressEdit()
        {
            UpdateFormats();
        }

        public static void Register()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(IPAddressEditName,
              typeof(IPAddressEdit), typeof(RepositoryItemIPAddressEdit),
                typeof(BaseSpinEditViewInfo), new ButtonEditPainter(), true));
        }

        public override string EditorTypeName
        {
            get
            {
                return IPAddressEditName;
            }
        }

        [Browsable(false)]
        public override FormatInfo EditFormat
        {
            get
            {
                return base.EditFormat;
            }
        }
        [Browsable(false)]
        public override FormatInfo DisplayFormat
        {
            get
            {
                return base.DisplayFormat;
            }
        }
        [Browsable(false)]
        public override MaskProperties Mask
        {
            get
            {
                return base.Mask;
            }
        }
        [Browsable(false)]
        public virtual new string EditMask
        {
            get
            {
                return "d.h.m.s";
            }
        }

        protected void UpdateFormats()
        {
            EditFormat.FormatString = EditMask;
            DisplayFormat.FormatString = EditMask;
            Mask.EditMask = EditMask;
        }
        public override string GetDisplayText(FormatInfo format, object editValue)
        {
            if (editValue is DateTime && IPAddressHelper.IsConvertible((DateTime)editValue))
                return IPAddressHelper.ToIPAddress((DateTime)editValue).ToString();

            if (editValue is IPv4Addr || editValue is string)
                return editValue.ToString();

            return GetDisplayText(null, new IPv4Addr("0.0.0.0"));
        }
        protected internal virtual string GetFormatMaskAccessFunction(string editMask, CultureInfo managerCultureInfo)
        {
            return GetFormatMask(editMask, managerCultureInfo);
        }
    }

    public class IPAddressEditMaskProperties : TimeEditMaskProperties
    {
        public IPAddressEditMaskProperties()
            : base()
        {
        }

        public virtual MaskManager CreatePatchedMaskManager()
        {
            CultureInfo managerCultureInfo = this.Culture;
            if (managerCultureInfo == null)
                managerCultureInfo = CultureInfo.CurrentCulture;

            string editMask = this.EditMask;
            if (editMask == null)
                editMask = string.Empty;

            return new IPAddressMaskManager(editMask, false, managerCultureInfo, true);
        }
    }

    public class IPAddressMaskManager : DateTimeMaskManagerCore
    {
        public IPAddressMaskManager(string mask, bool isOperatorMask, CultureInfo culture, bool allowNull)
            : base(mask, isOperatorMask, culture, allowNull)
        {
            fFormatInfo = new IPAddressMaskFormatInfo(mask, this.fInitialDateTimeFormatInfo);
        }

        public override void SetInitialEditText(string initialEditText)
        {
            KillCurrentElementEditor();
            DateTime? initialEditValue = new DateTime(0);

            if (!string.IsNullOrEmpty(initialEditText))
                try
                {
                    initialEditValue = IPAddressHelper.ToDateTime(new IPv4Addr(initialEditText));
                }
                catch
                {
                }

            SetInitialEditValue(initialEditValue);
        }
    }

    public class IPAddressMaskFormatInfo : DateTimeMaskFormatInfo
    {
        public IPAddressMaskFormatInfo(string mask, DateTimeFormatInfo dateTimeFormatInfo)
            : base(mask, dateTimeFormatInfo)
        {
            for (int i = 0; i < Count; i++)
            {
                if (innerList[i] is DateTimeMaskFormatElement_d)
                    innerList[i] = new IPAddressMaskFormatElement("d", dateTimeFormatInfo, 1);

                if (innerList[i] is DateTimeMaskFormatElement_h12)
                    innerList[i] = new IPAddressMaskFormatElement("h", dateTimeFormatInfo, 2);

                if (innerList[i] is DateTimeMaskFormatElement_Min)
                    innerList[i] = new IPAddressMaskFormatElement("m", dateTimeFormatInfo, 3);

                if (innerList[i] is DateTimeMaskFormatElement_s)
                    innerList[i] = new IPAddressMaskFormatElement("s", dateTimeFormatInfo, 4);
            }
        }
    }

    public class IPAddressMaskFormatElement : DateTimeNumericRangeFormatElementEditable
    {
        private int ipAddressPart = -1;

        public IPAddressMaskFormatElement(string mask, DateTimeFormatInfo datetimeFormatInfo, int IPAddressPartNumber)
            : base(mask, datetimeFormatInfo, DateTimePart.Time)
        {
            ipAddressPart = IPAddressPartNumber - 1;
        }

        public override DateTimeElementEditor CreateElementEditor(DateTime editedDateTime)
        {
            return new DateTimeNumericRangeElementEditor(GetIpAddressPart(editedDateTime, ipAddressPart), 0, 255, 1, 3);
        }

        public override DateTime ApplyElement(int result, DateTime editedDateTime)
        {
            string[] ipSplitted = IPAddressHelper.ToIPAddress(editedDateTime).ToStringArray();

            for (int i = 0; i < ipSplitted.Length; i++)
            {
                if (i == ipAddressPart)
                    ipSplitted[i] = String.Format("{0:d3}", result);
                else
                    ipSplitted[i] = String.Format("{0:d3}", Convert.ToInt16(ipSplitted[i]));
            }

            return IPAddressHelper.ToDateTime(new IPv4Addr(String.Join(".", ipSplitted)));
        }

        public override string Format(DateTime formattedDateTime)
        {
            return GetIpAddressPart(formattedDateTime, ipAddressPart).ToString();
        }

        protected virtual int GetIpAddressPart(DateTime dt, int partNumber)
        {
            if (partNumber < 0 || partNumber > 3)
                throw new Exception("Given part number is out of IPv4 address parts");

            string[] ipSplitted = IPAddressHelper.ToIPAddress(dt).ToStringArray();
            return Convert.ToInt16(ipSplitted[partNumber]);
        }
    }

    public class IPAddressHelper
    {
        public static DateTime ToDateTime(IPv4Addr ip)
        {
            if (ip == null)
                ip = new IPv4Addr("0.0.0.0");

            byte[] ipParts = ip.ToByteArray();
            string ipStr = "";

            for (int i = 0; i < ipParts.Length; i++)
                ipStr += String.Format("{0:d3}", ipParts[i]);

            return new DateTime(Int64.Parse(ipStr));
        }

        public static IPv4Addr ToIPAddress(DateTime dt)
        {
            if (dt.Ticks == 0)
                return new IPv4Addr("0.0.0.0");

            string ip = "";

            string strIP = dt.Ticks.ToString();
            while (strIP.Length < 12)
                strIP = "0" + strIP;

            while (strIP != "")
            {
                ip += Int16.Parse(strIP.Substring(0, 3)).ToString() + ".";
                strIP = strIP.Remove(0, 3);
            }
            ip = ip.Remove(ip.Length - 1);

            return new IPv4Addr(ip);
        }

        public static bool IsConvertible(DateTime dt)
        {
            if (dt.Ticks > 255255255255)
                return false;

            return true;
        }
    }

    public class IPv4Addr
    {
        private byte ip1;
        private byte ip2;
        private byte ip3;
        private byte ip4;

        public IPv4Addr()
        {
            ip1 = 0;
            ip2 = 0;
            ip3 = 0;
            ip4 = 0;
        }
        public IPv4Addr(string ipAddress)
        {
            if (String.IsNullOrWhiteSpace(ipAddress))
                ipAddress = "0.0.0.0";

            string[] ip = ipAddress.Split('.');
            TypeConverter tc = new TypeConverter();
            ip1 = (Byte.TryParse(ip[0], out ip1)) ? ip1 : (byte)0;
            ip2 = (Byte.TryParse(ip[1], out ip2)) ? ip2 : (byte)0;
            ip3 = (Byte.TryParse(ip[2], out ip3)) ? ip3 : (byte)0;
            ip4 = (Byte.TryParse(ip[3], out ip4)) ? ip4 : (byte)0;
        }
        public IPv4Addr(byte AddressPart1, byte AddressPart2, byte AddressPart3, byte AddressPart4)
        {
            ip1 = AddressPart1;
            ip2 = AddressPart2;
            ip3 = AddressPart3;
            ip4 = AddressPart4;
        }

        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}.{3}", ip1, ip2, ip3, ip4);
        }

        public string[] ToStringArray()
        {
            return new string[4] { ip1.ToString(), ip2.ToString(), ip3.ToString(), ip4.ToString() };
        }

        public byte[] ToByteArray()
        {
            return new byte[4] { ip1, ip2, ip3, ip4 };
        }
    }

}
