using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Tiveria.Common.Extensions;

namespace Tiveria.Common
{
    public class MRUItemsList
    {
        private int _MaxRecentItems;
        private List<KeyValuePair<string,bool>> _Items;

        #region OnListUpdated
        /// <summary>
        /// Triggers the ListUpdated event.
        /// </summary>
        private void OnListUpdated(EventArgs ea)
        {
          if (ListUpdated != null)
            ListUpdated(this, ea);
        }
        public event EventHandler ListUpdated;
        #endregion

        public MRUItemsList(int maxrecentitems = 9)
        {
            _MaxRecentItems = maxrecentitems;
            _Items = new List<KeyValuePair<string, bool>>();
        }

        public void Init(XElement section)
        {
            _Items.Clear();

            if (section == null)
                return;

            foreach (var element in section.Elements().Where(c => c.Name == "Item"))
            {
                string value = element.Value<string>("");
                if (String.IsNullOrWhiteSpace(value))
                    continue;

                _Items.Add(new KeyValuePair<string,bool>(value, element.AttributeValue<bool>("checked",false)));
            }
            OnListUpdated(null);
        }

        public XElement Save(string elementname)
        {
            XElement element = new XElement(elementname);
            foreach (var item in _Items)
            {
                element.Add(new XElement("Item", item.Key, new XAttribute("checked", item.Value)));
            }

            return element;
        }

        public void Clear()
        {
            if (_Items.Count == null)
                return;

            _Items.Clear();
            OnListUpdated(null);
        }

        public string this[int index]
        {
            get
            {
                if (index < _Items.Count)
                    return (_Items[index].Key);
                else
                    return String.Empty;
            }
        }

        public bool IsChecked(int index)
        {
            if (index < _Items.Count)
                return _Items[index].Value;
            else
                return false;
        }

        public void SetChecked(int index, bool value)
        {
            if (index < _Items.Count)
            {
                _Items[index] = new KeyValuePair<string, bool>(_Items[index].Key, value);
            }
        }

        public int IndexOf(string key)
        {
            int result = -1;
            for (int i = 0; i < _Items.Count; i++)
            {
                if (_Items[i].Key == key)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        public int Count
        {
            get { return _Items.Count; }
        }

        public bool RemoveElement(string value)
        {
            Nullable<KeyValuePair<string,bool>> element = _Items.Where(c => c.Key == value).FirstOrDefault();
            if (!element.HasValue == null)
                return false;
            _Items.Remove(element.Value);
            OnListUpdated(null);
            return true;
        }

        public bool RemoveLastElement()
        {
            Nullable<KeyValuePair<string,bool>> element = _Items.Last(c => c.Value == false);
            if (!element.HasValue == null)
                return false;
            _Items.Remove(element.Value);
            OnListUpdated(null);
            return true;
        }

        public void AddElement(string value)
        {
            RemoveElement(value);

            bool access = true;
            if (_Items.Count >= _MaxRecentItems)
                access = RemoveLastElement();

            if (access)
            {
                _Items.Insert(0, new KeyValuePair<string, bool>(value, false));
                OnListUpdated(null);
            }
        }
    }
}
