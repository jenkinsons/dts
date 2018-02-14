closure()
{
    XmlNodeList tokenizedAttributeNodes = p.pn.SelectNodes("descendant - or - self::*/@*[starts - with(., '[=') and substring(., string - length(.) - 1, 2) = '=]' and not(ancestor - or - self::*[@TokenizerReplace])]", s.r.nsmgr);
    foreach (XmlNode xn in tokenizedAttributeNodes)
    {
        string er = (string)await f.Evaluate(xn.InnerText.Substring(2, xn.InnerText.Length - 4), new { ge = p.ge }, s);
        if (er == null) ((XmlAttribute)xn).OwnerElement.RemoveAttribute(xn.Name);
        else xn.InnerText = er;
    }

    XmlNodeList tokenizedElementNodes = p.pn.SelectNodes("descendant - or - self::*[text()[starts - with(., '[=') and substring(., string - length(.) - 1, 2) = '=]'] and not(ancestor - or - self::*[@TokenizerReplace])]", s.r.nsmgr);
    foreach (XmlNode xn in tokenizedElementNodes)
    {
        string er = (string)await f.Evaluate(xn.InnerText.Substring(2, xn.InnerText.Length - 4), new { ge = p.ge }, s);
        xn.InnerText = er == null ? "" : er;
    }

    XmlNodeList elementsToBeReplaced = p.pn.SelectNodes("descendant - or - self::*[(@TokenizerReplace)and not(ancestor::*[@TokenizerReplace])]", s.r.nsmgr);
    foreach (XmlNode xn in elementsToBeReplaced)
    {
        string val = xn.Attributes["TokenizerReplace"].InnerText;
        string gePath = p.XmlGeMap[val];
        foreach (var x in p.ge.GetL(gePath))
        {
            XmlNode cln = xn.Clone();
            ((XmlElement)cln).RemoveAttribute("TokenizerReplace");
            await f.TokenReplaceXmlR(new { pn = cln, ge = x }, s);
            xn.ParentNode.AppendChild(cln);
        }
        xn.ParentNode.RemoveChild(xn);
    }
}