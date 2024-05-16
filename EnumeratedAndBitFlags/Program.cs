using System.Reflection;
using System.Text;

ParseEnumFromString();
InspectEnumTypeInfo();
OperatorsWithEnum();
FileBitFlags();
BitFlagsEnum();
SimulateFlagToString();
SimulateFlagParse();

void InspectEnumTypeInfo()
{
    Console.WriteLine("____{0}()____", nameof(InspectEnumTypeInfo));

    Type colorType = typeof(Color);

    Type underlyingType = colorType.GetEnumUnderlyingType();
    Console.WriteLine("Underlying type of {0} is {1}", colorType, underlyingType);

    underlyingType = Enum.GetUnderlyingType(colorType);
    Console.WriteLine("Underlying type of {0} is {1}", colorType, underlyingType);

    //ArgumentException
    //Console.WriteLine(typeof(Program).GetEnumUnderlyingType());
}

void ParseEnumFromString()
{
    Console.WriteLine("____{0}()____", nameof(ParseEnumFromString));

    Color color;

    color = Enum.Parse<Color>("white", true);
    System.Console.WriteLine("Parse \"{0}\" to Color: {1}", "white", color);

    color = Enum.Parse<Color>("0");
    System.Console.WriteLine("Parse \"{0}\" to Color: {1}", "0", color);

    //ArgumentException
    //color =  Enum.Parse<Color>("unknown");
    //System.Console.WriteLine("Parse \"{0}\" to Color: {1}", "unknown", color);

    color = Enum.Parse<Color>("123");
    System.Console.WriteLine("Parse \"{0}\" to Color: {1}", "123", color);
}

void OperatorsWithEnum()
{
    Console.WriteLine("____{0}()____", nameof(OperatorsWithEnum));

    Color white = Color.White;
    Color red = Color.Red;
    Console.WriteLine("Result of {0} > {1} = {2}", white, red, white > red);

    Console.WriteLine("Result of ++{0} = {1}", white, ++white);
    white--;

    Console.WriteLine("Result of ({0}){1} = {2}", typeof(OtherColor), white, (OtherColor)white);

    Console.WriteLine("{0}.ToString(\"d\") = {1}", white, white.ToString("d"));
    Console.WriteLine("{0}.ToString(\"f\") = {1}", white, white.ToString("f"));
    Console.WriteLine("{0}.ToString(\"x\") = {1}", white, white.ToString("x"));

    OtherColor otherColor = OtherColor.LightRed;
    Console.WriteLine("{0}.ToString(\"x\") = {1}", otherColor, otherColor.ToString("x"));

    String format = Enum.Format(typeof(Color), white, "f");
    Console.WriteLine("Enum.Format(typeof(Color), {0}, \"f\") = {1}", white, format);

    format = Enum.Format(typeof(Color), 3L, "f");
    Console.WriteLine("Enum.Format(typeof(Color), (long) 3, \"f\") = {0}", format);

    Console.WriteLine("Convert 1 to {0} = {1}", typeof(EnumWithMultipleNamesSameValue), (EnumWithMultipleNamesSameValue)1);
    Console.WriteLine("Convert 2 to {0} = {1}", typeof(EnumWithMultipleNamesSameValue), (EnumWithMultipleNamesSameValue)2);
}

void FileBitFlags()
{
    Console.WriteLine("____{0}()____", nameof(FileBitFlags));

    string file = Assembly.GetEntryAssembly()?.Location!;
    FileAttributes attributes = File.GetAttributes(file);
    System.Console.WriteLine("Is is hidden? {0}", (attributes & FileAttributes.Hidden) != 0);
}

void BitFlagsEnum()
{
    Console.WriteLine("____{0}()____", nameof(BitFlagsEnum));

    Actions readAndDelete = Actions.Read | Actions.Delete;
    System.Console.WriteLine("Actions.Read | Actions.Delete = {0:G} ({0:D})", readAndDelete);

    Actions queryAndSync = Actions.Query | Actions.Sync;
    System.Console.WriteLine("Actions.Query | Actions.Sync = {0:G} ({0:D})", queryAndSync);
}

void SimulateFlagToString()
{
    Console.WriteLine("____{0}()____", nameof(SimulateFlagToString));

    NoFlagsActions[] enumValues = Enum.GetValues<NoFlagsActions>().OrderDescending().ToArray();

    NoFlagsActions actions = NoFlagsActions.Query | NoFlagsActions.Write;
    System.Console.WriteLine(FlagsToString(actions));

    actions = NoFlagsActions.None | NoFlagsActions.Sync;
    System.Console.WriteLine(FlagsToString(actions));

    actions = 0;
    System.Console.WriteLine(FlagsToString(actions));

    actions = (NoFlagsActions)188;
    System.Console.WriteLine(FlagsToString(actions));

    actions = NoFlagsActions.Read | NoFlagsActions.Delete;
    System.Console.WriteLine(FlagsToString(actions));

    System.Console.WriteLine("actions.ToString(\"f\") = {0}", actions.ToString("f"));
}
string FlagsToString(NoFlagsActions actions)
{
    StringBuilder sb = new StringBuilder();
    NoFlagsActions[] enumValues = Enum.GetValues<NoFlagsActions>().OrderDescending().ToArray();
    NoFlagsActions processingValue = actions;
    int valueIndex = 0;
    while (processingValue != 0 && valueIndex < enumValues.Length)
    {
        NoFlagsActions enumValue = enumValues[valueIndex++];
        if ((processingValue & enumValue) == enumValue)
        {
            sb.AppendFormat("{1}{0:G}", enumValue, sb.Length != 0 ? ", " : string.Empty);
            processingValue ^= enumValue;
        }
    }
    if (processingValue != 0)
        return actions.ToString("D");
    if (actions != 0)
        return sb.ToString();
    if (Enum.IsDefined<NoFlagsActions>(0))
        return Enum.Format(typeof(NoFlagsActions), 0, "G");
    return "0";
}
void SimulateFlagParse()
{
    Console.WriteLine("____{0}()____", nameof(SimulateFlagParse));

    System.Console.WriteLine("Parse {0} by Enum.Parse()", nameof(NoFlagsActions));
    System.Console.WriteLine("Parse \"Read, Write\" result is {0}", Enum.Parse<NoFlagsActions>("Read, Write"));
    System.Console.WriteLine("Parse \"2\" result is {0}", Enum.Parse<NoFlagsActions>("2"));
    System.Console.WriteLine("Parse \"5\" result is {0}", Enum.Parse<NoFlagsActions>("5"));

    System.Console.WriteLine("Parse {0} by FlagsParse()", nameof(NoFlagsActions));
    System.Console.WriteLine("Parse \"Read, Write\" result is {0}", FlagsParse("Read, Write"));
    System.Console.WriteLine("Parse \"2\" result is {0}", FlagsParse("2"));
    System.Console.WriteLine("Parse \"5\" result is {0}", FlagsParse("5"));
}
NoFlagsActions FlagsParse(string str, bool ignoreCase = false)
{
    ArgumentNullException.ThrowIfNullOrEmpty(str);
    if (int.TryParse(str, out var num))
        return Enum.Parse<NoFlagsActions>(num.ToString());
    NoFlagsActions result = 0;
    NoFlagsActions[] enumValues = Enum.GetValues<NoFlagsActions>();
    str = str.Trim();
    string[] parts = str.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    foreach (string part in parts)
    {
        var found = false;
        foreach (var enumValue in enumValues)
        {
            if (part.Equals(enumValue.ToString("G"), ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                result |= enumValue;
                found = true;
                break;
            }
        }
        if (!found) throw new ArgumentException($"Enum {nameof(NoFlagsActions)} not define {part} symbol.", nameof(str));
    }
    return result;
}

enum OtherColor
{
    LightWhite,
    LightRed
}
enum EnumWithMultipleNamesSameValue
{
    A = 1,
    B = 1,
    D = 2,
    E = 2,
}

[Flags]
enum Actions
{
    None = 0,
    Read = 0x0001,
    Write = 0x0002,
    ReadWrite = Read | Write,
    Delete = 0x0004,
    Query = 0x0008,
    Sync = 0x0010,
    All = Read | Write | Delete | Sync | Query
}

enum NoFlagsActions
{
    None = 0,
    Read = 0x0001,
    Write = 0x0002,
    ReadWrite = Read | Write,
    Delete = 0x0004,
    Query = 0x0008,
    Sync = 0x0010,
    All = Read | Write | Delete | Sync | Query
}