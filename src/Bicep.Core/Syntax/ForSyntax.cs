// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Parsing;
using System;

namespace Bicep.Core.Syntax
{
    public class ForSyntax : SyntaxBase
    {
        public ForSyntax(
            Token openSquare,
            Token forKeyword,
            SyntaxBase itemVariableOrVariableBlock,
            SyntaxBase inKeyword,
            SyntaxBase expression,
            SyntaxBase colon,
            SyntaxBase body,
            SyntaxBase closeSquare)
        {
            AssertTokenType(openSquare, nameof(openSquare), TokenType.LeftSquare);
            AssertKeyword(forKeyword, nameof(forKeyword), LanguageConstants.ForKeyword);
            AssertSyntaxType(itemVariableOrVariableBlock, nameof(itemVariableOrVariableBlock), typeof(LocalVariableSyntax), typeof(ForVariableBlockSyntax));
            AssertSyntaxType(inKeyword, nameof(inKeyword), typeof(Token), typeof(SkippedTriviaSyntax));
            AssertKeyword(inKeyword as Token, nameof(inKeyword), LanguageConstants.InKeyword);
            AssertSyntaxType(colon, nameof(colon), typeof(Token), typeof(SkippedTriviaSyntax));
            AssertTokenType(colon as Token, nameof(colon), TokenType.Colon);
            AssertSyntaxType(closeSquare, nameof(closeSquare), typeof(Token), typeof(SkippedTriviaSyntax));
            AssertTokenType(closeSquare as Token, nameof(closeSquare), TokenType.RightSquare);
            
            this.OpenSquare = openSquare;
            this.ForKeyword = forKeyword;
            this.VariableSection = itemVariableOrVariableBlock;
            this.InKeyword = inKeyword;
            this.Expression = expression;
            this.Colon = colon;
            this.Body = body;
            this.CloseSquare = closeSquare;
        }

        public Token OpenSquare { get; }

        public Token ForKeyword { get; }

        public SyntaxBase VariableSection { get; }

        public SyntaxBase InKeyword { get; }

        public SyntaxBase Expression { get; }

        public SyntaxBase Colon { get; }

        public SyntaxBase Body { get; }

        public SyntaxBase CloseSquare { get; }

        public override void Accept(ISyntaxVisitor visitor) => visitor.VisitForSyntax(this);

        public override TextSpan Span => TextSpan.Between(this.OpenSquare, this.CloseSquare);

        public LocalVariableSyntax ItemVariable => this.VariableSection switch
        {
            LocalVariableSyntax itemVariable => itemVariable,
            ForVariableBlockSyntax block => block.ItemVariable,
            _ => throw new NotImplementedException($"Unexpected loop variable section type '{this.VariableSection.GetType().Name}'.")
        };

        public LocalVariableSyntax? IndexVariable => this.VariableSection switch
        {
            LocalVariableSyntax itemVariable => null,
            ForVariableBlockSyntax block => block.IndexVariable,
            _ => throw new NotImplementedException($"Unexpected loop variable section type '{this.VariableSection.GetType().Name}'.")
        };
    }
}
