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
        _workflows.Add("A", new Workflow("A", []));
        _workflows.Add("R", new Workflow("R", []));

        var dict = new Dictionary<string, Range>()
        {
            { "x", new Range(1, 4001) },
            { "m", new Range(1, 4001) },
            { "a", new Range(1, 4001) },
            { "s", new Range(1, 4001) },
        };

        return Combinations(dict, _workflows["in"]).ToString();
    }

    private long Combinations(Dictionary<string, Range> ranges, Workflow workflow)
    {
        if (workflow.Name == "R")
        {
            return 0;
        }

        if (workflow.Name == "A")
        {
            return ranges
                .Aggregate(1L, (acc, range) => acc * (range.Value.End.Value - range.Value.Start.Value));
        }

        var rules = workflow.Rules;

        long total = 0;

        foreach (var rule in rules)
        {
            if (rule.Category is null)
            {
                total += Combinations(ranges, _workflows[rule.Result]);
                break;
            }

            var (start, end) = (ranges[rule.Category].Start.Value, ranges[rule.Category].End.Value);

            Range t;
            Range f;
            if (rule.Comparison == "<" && rule.Value is not null)
            {
                t = new Range(start, rule.Value.Value);
                f = new Range(rule.Value.Value, end);
            }
            else
            {
                t = new Range(rule.Value.Value + 1, end);
                f = new Range(start, rule.Value.Value + 1);
            }

            if (t.Start.Value < t.End.Value)
            {
                var dict = ranges.ToDictionary();
                dict[rule.Category] = t;
                total += Combinations(dict, _workflows[rule.Result]);
            }

            if (f.Start.Value < f.End.Value)
            {
                ranges = ranges.ToDictionary();
                ranges[rule.Category] = f;
            }
        }

        return total;
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

    class Rule
    {
        public string? Category { get; set; }
        public string? Comparison { get; set; }
        public int? Value { get; set; }
        public string Result { get; set; }
        public bool IsDefault { get; set; } = false;

        public Rule(string result)
        {
            Result = result;
            IsDefault = true;
        }

        public Rule(string category, string comparison, int value, string result)
        {
            Category = category;
            Comparison = comparison;
            Value = value;
            Result = result;
        }

        public static Rule Parse(string input)
        {
            if (!input.Contains(':'))
            {
                return new Rule(input);
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
            foreach (var rule in Rules)
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
