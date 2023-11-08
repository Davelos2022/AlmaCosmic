using AirFishLab.ScrollingList.ContentManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AirFishLab.ScrollingList.Demo
{
    public class IntListBox : ListBox
    {
        public int Content { get; private set; }

        protected override void UpdateDisplayContent(IListContent listContent)
        {
            Content = ((IntListContent)listContent).Value;
        }
    }
}
