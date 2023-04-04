using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InboxButton : MonoBehaviour
{
    public int id;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;

    public void SetTitle(string _title) {
        title.text = _title;
    }

    public void SetDescription(string _description) {
        description.text = _description;
    }

    public void SetID(int _id) {
        id = _id;
    }

    public void ShowInbox() {
        InboxManager.Instance.SetUpInboxDetails(id);
    }
}
