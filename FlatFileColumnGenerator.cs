closure()
{
    int i = 0;
    foreach (var x in p.ge.GetL(""))
    {
        XmlNode cln = p.cn.Clone();
        ((XmlElement)cln).RemoveAttribute("TokenizerReplace");
        await f.TokenReplaceXmlR(new { pn = cln, ge = x }, s);
        p.cn.ParentNode.AppendChild(cln);
        i++;
    }
    p.cn.ParentNode.RemoveChild(p.cn);
}