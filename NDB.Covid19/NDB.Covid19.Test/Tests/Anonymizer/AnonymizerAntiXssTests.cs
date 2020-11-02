using FluentAssertions;
using Xunit;
using static NDB.Covid19.Utils.Anonymizer;

namespace NDB.Covid19.Test.Tests.Anonymizer
{
    public class AnonymizerAntiXssTests
    {
        [Fact]
        public void MaliciousCodeShouldBeEncoded()
        {
            var script = "<SCRIPT SRC=http://xss.rocks/xss.js></SCRIPT>";
            RedactText(script)
                .Should().NotBe(script).And.Be("");
            script =
                "javascript:/*--></title></style></textarea></script></xmp><svg/onload='+/\"/+/onmouseover=1/+/[*/[]/+alert(1)//'>";
            RedactText(script)
                .Should().NotBe(script).And.Be("javascript:/*--&amp;gt;");
            script = "<IMG SRC=\"javascript:alert('XSS');\">";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "<IMG SRC=javascript:alert('XSS')>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "<IMG SRC=JaVaScRiPt:alert('XSS')>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "<IMG SRC=javascript:alert(&quot;XSS&quot;)>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "<IMG SRC=`javascript:alert(\"RSnake says, 'XSS'\")`>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "\\<a onmouseover=\"alert(document.cookie)\"\\>xxs link\\</a\\>";
            RedactText(script)
                .Should().NotBe(script).And.Be("\\&lt;a&gt;xxs link\\&lt;/a&gt;");
            script = "\\<a onmouseover=alert(document.cookie)\\>xxs link\\</a\\>";
            RedactText(script)
                .Should().NotBe(script).And.Be("\\&lt;a&gt;xxs link\\&lt;/a&gt;");
            script = "<IMG \"\"\"><SCRIPT>alert(\"XSS\")</SCRIPT>\"\\>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img&gt;&amp;quot;\\&amp;gt;");
            script = "<IMG SRC=javascript:alert(String.fromCharCode(88,83,83))>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "<IMG SRC= onmouseover=\"alert('xxs')\">";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;onmouseover=&amp;quot;alert(&#39;xxs&#39;)&amp;quot;&quot;&gt;");
            script = "<IMG SRC=/ onerror=\"alert(String.fromCharCode(88,83,83))\"></img>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;/&quot;&gt;");
            script = "<IMG SRC=&#106;&#97;&#118;&#97;&#115;&#99;&#114;&#105;&#112;&#116;&#58;&#97;&#108;&#101;&#114;&#116;&#40;&#39;&#88;&#83;&#83;&#39;&#41;>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "<IMG SRC=&#x6A&#x61&#x76&#x61&#x73&#x63&#x72&#x69&#x70&#x74&#x3A&#x61&#x6C&#x65&#x72&#x74&#x28&#x27&#x58&#x53&#x53&#x27&#x29>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "<IMG SRC=\"jav&#x09;ascript:alert('XSS');\">";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "<IMG SRC=\" &#14;  javascript:alert('XSS');\">";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;img src=&quot;&quot;&gt;");
            script = "<BODY onload!#$%&()*~+-_.,:;?@[/|\\]^`=alert(\"XSS\")>";
            RedactText(script)
                .Should().NotBe(script).And.Be("");
            script = "<<SCRIPT>alert(\"XSS\");//\\<</SCRIPT>";
            RedactText(script)
                .Should().NotBe(script).And.Be("&amp;lt;");
            script = "<SCRIPT SRC=http://xss.rocks/xss.js?< B >";
            RedactText(script)
                .Should().NotBe(script).And.Be("");
            script = "<SCRIPT SRC=//xss.rocks/.j>";
            RedactText(script)
                .Should().NotBe(script).And.Be("");
            script = "<IMG SRC=\"`<javascript:alert>`('XSS')\"";
            RedactText(script)
                .Should().NotBe(script).And.Be("&amp;lt;IMG SRC=&amp;quot;``(&#39;XSS&#39;)&amp;quot;");
            script = "<iframe src=http://xss.rocks/scriptlet.html <";
            RedactText(script)
                .Should().NotBe(script).And.Be("&amp;lt;iframe src=http://xss.rocks/scriptlet.html &amp;lt;");
            script = "\\\";alert('XSS');//";
            RedactText(script)
                .Should().NotBe(script).And.Be("\\&amp;quot;;alert(&#39;XSS&#39;);//");
            script = "<BR SIZE=\"&{alert('XSS')}\">";
            RedactText(script)
                .Should().NotBe(script).And.Be("&lt;br size=&quot;&amp;amp;{alert(&#39;XSS&#39;)}&quot;&gt;&#13;&#10;");
            script = "<STYLE>BODY{-moz-binding:url(\"http://xss.rocks/xssmoz.xml#xss\")}</STYLE>";
            RedactText(script)
                .Should().NotBe(script).And.Be("");
            script = "exp/*<A STYLE='no\\xss:noxss(\"*//*\");\r\nxss:ex/*XSS*//*/*/pression(alert(\"XSS\"))'>";
            RedactText(script)
                .Should().NotBe(script).And.Be("exp/*&lt;a&gt;&lt;/a&gt;");
        }
    }
}
