using System.Text;
using NFluent;

namespace StringParser;

public class AnswerTest
{
    [Fact(DisplayName = "Returns expected results using simple strings")]
    public void Should_Return_Expected_When_Simple_Strings()
    {
        var result1 = Answer.Check("()");
        Check.That(result1).IsTrue();

        var result2 = Answer.Check("[]");
        Check.That(result2).IsTrue();

        var result3 = Answer.Check("()()");
        Check.That(result3).IsTrue();

        var result4 = Answer.Check("[][]");
        Check.That(result4).IsTrue();

        var result5 = Answer.Check("()[]");
        Check.That(result5).IsTrue();

        var result6 = Answer.Check("[]()");
        Check.That(result6).IsTrue();

        var result7 = Answer.Check("(())");
        Check.That(result7).IsTrue();

        var result8 = Answer.Check("[[]]");
        Check.That(result8).IsTrue();

        var result9 = Answer.Check("([])");
        Check.That(result9).IsTrue();

        var result10 = Answer.Check("[()]");
        Check.That(result10).IsTrue();

        var result11 = Answer.Check("(()[])");
        Check.That(result11).IsTrue();

        var result12 = Answer.Check("(");
        Check.That(result12).IsFalse();

        var result13 = Answer.Check("[");
        Check.That(result13).IsFalse();

        var result14 = Answer.Check(")");
        Check.That(result14).IsFalse();

        var result15 = Answer.Check("]");
        Check.That(result15).IsFalse();

        var result16 = Answer.Check("((");
        Check.That(result16).IsFalse();

        var result17 = Answer.Check("([");
        Check.That(result17).IsFalse();

        var result18 = Answer.Check("[(");
        Check.That(result18).IsFalse();

        var result19 = Answer.Check("[[");
        Check.That(result19).IsFalse();

        var result20 = Answer.Check("))");
        Check.That(result20).IsFalse();

        var result21 = Answer.Check(")]");
        Check.That(result21).IsFalse();

        var result22 = Answer.Check("])");
        Check.That(result22).IsFalse();

        var result23 = Answer.Check("]]");
        Check.That(result23).IsFalse();

        var result24 = Answer.Check(")(");
        Check.That(result24).IsFalse();

        var result25 = Answer.Check("][");
        Check.That(result25).IsFalse();

        var result26 = Answer.Check("(]");
        Check.That(result26).IsFalse();

        var result27 = Answer.Check("[)");
        Check.That(result27).IsFalse();

        var result28 = Answer.Check("((])");
        Check.That(result28).IsFalse();

        var result29 = Answer.Check("(()]");
        Check.That(result29).IsFalse();

        var result30 = Answer.Check("([))");
        Check.That(result30).IsFalse();

        var result31 = Answer.Check("[())");
        Check.That(result31).IsFalse();

        var result32 = Answer.Check("([])[)");
        Check.That(result32).IsFalse();

        var result33 = Answer.Check("[)([])");
        Check.That(result33).IsFalse();

        var result34 = Answer.Check("([)]");
        Check.That(result34).IsFalse();
    }

    [Fact(DisplayName = "'' returns true")]
    public void Should_Return_True_When_String_Empty()
    {
        var result = Answer.Check(string.Empty);

        Check.That(result).IsTrue();
    }

    
    [Fact(DisplayName = "Gets the expected result using a long string (in time and without a stack overflow)")]
    public void Should_Return_False_When_Invalid_Long_String()
    {
        var validStr = GenerateBigValidStr();
        var result1 = Answer.Check(validStr);
        Check.That(result1).IsTrue();

        var unvalidStr = GenerateBigUnvalidStr(validStr);
        var result2 = Answer.Check(unvalidStr);
        Check.That(result2).IsFalse();
    }

    private static string GenerateBigValidStr()
    {
        var size = 10_000;
        char[] characters = ['(', '['];
        var r = new Random();

        var sb = new StringBuilder(size);
        sb.Append(characters[r.Next(2)]);

        for (int i = 0; i < size; i++)
        {
            var openOrClose = r.Next(2);

            if (openOrClose == 0 && i < 9_000)
            {
                sb.Append(characters[r.Next(2)]);
            }
            else
            {
                CloseStr(sb);
            }
        }

        return sb.ToString();
    }

    private static void CloseStr(StringBuilder sb)
    {
        var current = sb.ToString().Reverse();
        var parentheseCounter = 0;
        var bracketCounter = 0;

        foreach (var c in current)
        {
            _ = c switch
            {
                '(' => parentheseCounter--,
                ')' => parentheseCounter++,
                '[' => bracketCounter--,
                ']' => bracketCounter++,
                _ => throw new NotImplementedException(),
            };

            if (parentheseCounter < 0)
            {
                sb.Append(')');
                break;
            }
            else if (bracketCounter < 0)
            {
                sb.Append(']');
                break;
            }
        }
    }

    private static string GenerateBigUnvalidStr(string validStr)
    {
        var r = new Random();
        var index = r.Next(0, validStr.Length);

        var unvalidStr = new StringBuilder(validStr);

        var newChar = unvalidStr[index] switch
        {
            '(' => new[] { ')', '[', ']' }[r.Next(0, 3)],
            ')' => new[] { '(', '[', ']' }[r.Next(0, 3)],
            '[' => new[] { ')', '(', ']' }[r.Next(0, 3)],
            ']' => new[] { ')', '[', '(' }[r.Next(0, 3)],
            _ => throw new NotImplementedException()
        };

        unvalidStr[index] = newChar;
        return unvalidStr.ToString();
    }

    [Fact(DisplayName = "No error if more closing than opening (Empty Stack)")]
    public void Should_Return_False_When_More_Closing_Than_Opening()
    {
        var result = Answer.Check("[])");

        Check.That(result).IsFalse();
    }

    [Fact(DisplayName = "'null' returns true")]
    public void Should_Return_True_When_String_Is_Null()
    {
        var result = Answer.Check(null!);

        Check.That(result).IsTrue();
    }
}