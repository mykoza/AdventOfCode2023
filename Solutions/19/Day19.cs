using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day19 : Solution
{
    protected override bool UseExample => false;
    private readonly List<Part> _acceptedParts = [];
    private readonly Dictionary<string, Workflow> _workflows = [];
    private readonly List<Part> _parts = [];

    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        var parsingWorkflows = true;
        foreach (var line in inputLines)
        {
            if (line == string.Empty)
            {
                parsingWorkflows = false;
                continue;
            }

            if (parsingWorkflows)
            {
                var workflow = Workflow.Parse(line);
                _workflows.Add(workflow.Name, workflow);
            }
            else
            {
                _parts.Add(Part.Parse(line));
            }
        }
    }

    protected override string LogicPart1()
    {
        var inWorkflow = _workflows["in"];

        foreach (var part in _parts)
        {
            var currentWorkflow = inWorkflow;
            while (true)
            {
                var res = currentWorkflow.Evaluate(part);

                if (res == "A")
                {
                    _acceptedParts.Add(part);
                    break;
                }
                else if (res == "R")
                {
                    break;
                }
                else
                {
                    currentWorkflow = _workflows[res];
                }
            }
        }

        return _acceptedParts.Sum(x => x.X + x.M + x.A + x.S).ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }

    record Part(int X, int M, int A, int S)
    {
        public static Part Parse(string input)
        {
            var split = input.Trim(['{', '}']).Split(',');
            var x = int.Parse(split[0][2..]);
            var m = int.Parse(split[1][2..]);
            var a = int.Parse(split[2][2..]);
            var s = int.Parse(split[3][2..]);

            return new Part(x, m, a, s);
        }
    }

    class Rule(string? category, string? comparison, int? value, string result)
    {
        public string? Category { get; set; } = category;
        public string? Comparison { get; set; } = comparison;
        public int? Value { get; set; } = value;
        public string Result { get; set; } = result;

        public static Rule Parse(string input)
        {
            if (!input.Contains(':'))
            {
                return new Rule(null, null, null, input);
            }

            var category = input[0].ToString();
            var comparison = input.Substring(1, 1);
            var value = int.Parse(input[2..input.IndexOf(':')]);
            var result = input[(input.IndexOf(':') + 1)..];

            return new Rule(category, comparison, value, result);
        }

        public bool AppliesTo(Part part)
        {
            if (Category is null)
            {
                return true;
            }

            var val = (int)part.GetType().GetProperty(Category.ToUpper())!.GetValue(part, null)!;

            if (Comparison == "<")
            {
                return val < Value;
            }
            else
            {
                return val > Value;
            }
        }
    }

    class Workflow(string name, List<Rule> rules)
    {
        public string Name { get; set; } = name;
        public List<Rule> Rules { get; set; } = rules;

        public static Workflow Parse(string input)
        {
            var name = input[..input.IndexOf('{')];
            var rawRules = input[input.IndexOf('{')..].Trim(['{', '}']).Split(',');

            var rules = rawRules.Select(Rule.Parse).ToList();

            return new Workflow(name, rules);
        }

        public string Evaluate(Part part)
        {
            foreach (var rule in rules)
            {
                if (rule.AppliesTo(part))
                {
                    return rule.Result;
                }
            }

            throw new Exception($"No rule applies to {part}");
        }
    }
}
