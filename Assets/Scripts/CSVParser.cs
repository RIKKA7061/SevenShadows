using System.Collections.Generic;
using System.Text;

//대사에 쉼표 ,가 들어가더라도 정상적으로 읽을 수 있도록 CSV 파서를 만들어줍니다.
public static class CSVParser
{
    public static List<string[]> Parse(string csv)
    {
        List<string[]> rows = new List<string[]>();

        List<string> currentRow = new List<string>();
        StringBuilder currentField = new StringBuilder();

        bool insideQuotes = false;

        for (int i = 0; i < csv.Length; i++)
        {
            char c = csv[i];

            if (c == '"')
            {
                if (insideQuotes &&
                    i + 1 < csv.Length &&
                    csv[i + 1] == '"')
                {
                    currentField.Append('"');
                    i++;
                }
                else
                {
                    insideQuotes = !insideQuotes;
                }
            }
            else if (c == ',' && !insideQuotes)
            {
                currentRow.Add(currentField.ToString());
                currentField.Clear();
            }
            else if ((c == '\n' || c == '\r') && !insideQuotes)
            {
                if (c == '\r' &&
                    i + 1 < csv.Length &&
                    csv[i + 1] == '\n')
                {
                    i++;
                }

                currentRow.Add(currentField.ToString());
                currentField.Clear();

                if (currentRow.Count > 0)
                {
                    rows.Add(currentRow.ToArray());
                }

                currentRow = new List<string>();
            }
            else
            {
                currentField.Append(c);
            }
        }

        if (currentField.Length > 0 || currentRow.Count > 0)
        {
            currentRow.Add(currentField.ToString());
            rows.Add(currentRow.ToArray());
        }

        return rows;
    }
}