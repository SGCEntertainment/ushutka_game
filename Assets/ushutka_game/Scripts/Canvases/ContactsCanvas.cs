using UnityEngine;

public class ContactsCanvas : MonoBehaviour
{
    const string vkUrl = "https://vk.com/damir_author";
    const string tgUrl = "https://t.me/author_damir";
    const string instUrl = "https://www.instagram.com/author_damir/?hl=ru";
    const string litnetUrl = "https://litnet.com/ru/book/dnevnik-kollektora-b96206";

    public void OpenLink(int i)
    {
        switch(i)
        {
            case 0: Application.OpenURL(vkUrl); break;
            case 1: Application.OpenURL(tgUrl); break;
            case 2: Application.OpenURL(instUrl); break;
            case 3: Application.OpenURL(litnetUrl); break;
        }
    }
}
