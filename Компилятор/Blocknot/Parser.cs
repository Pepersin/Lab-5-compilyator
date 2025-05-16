using System;
using System.Collections.Generic;

public class Parser
{
    private readonly List<Token> tokens;
    private int pos;
    private List<string> poliz;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        pos = 0;
        poliz = new List<string>();
    }

    private Token Current => pos < tokens.Count ? tokens[pos] : tokens[tokens.Count - 1];

    private void Eat(TokenType type)
    {
        if (Current.Type == type)
        {
            pos++;
        }
        else
        {
            throw new Exception($"Ожидалось: {type}, найдено: {Current.Type}");
        }
    }

    public List<string> Parse()
    {
        poliz.Clear();
        ParseE();
        if (Current.Type != TokenType.EOF) throw new Exception("Лишние символы после выражения.");
        return poliz;
    }

    private void ParseE()
    {
        ParseT();
        ParseA();
    }

    private void ParseA()
    {
        if (Current.Type == TokenType.Plus)
        {
            Eat(TokenType.Plus);
            ParseT();
            poliz.Add("+");
            ParseA();
        }
        else if (Current.Type == TokenType.Minus)
        {
            Eat(TokenType.Minus);
            ParseT();
            poliz.Add("-");
            ParseA();
        }
    }

    private void ParseT()
    {
        ParseO();
        ParseB();
    }

    private void ParseB()
    {
        if (Current.Type == TokenType.Multiply)
        {
            Eat(TokenType.Multiply);
            ParseO();
            poliz.Add("*");
            ParseB();
        }
        else if (Current.Type == TokenType.Divide)
        {
            Eat(TokenType.Divide);
            ParseO();
            poliz.Add("/");
            ParseB();
        }
    }

    private void ParseO()
    {
        if (Current.Type == TokenType.Number)
        {
            poliz.Add(Current.Value);
            Eat(TokenType.Number);
        }
        else if (Current.Type == TokenType.LParen)
        {
            Eat(TokenType.LParen);
            ParseE();
            Eat(TokenType.RParen);
        }
        else
        {
            throw new Exception($"Ожидалось число или '(', найдено: {Current.Type}");
        }
    }
}
