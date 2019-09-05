grammar OutputExpression;

expr: ifexpr                                 # IfExpression
	| function                               # FunctionExpression
    | literal                                # LiteralExpression
    | field                                  # FieldExpression
	| '!' expr                               # NotExpression
	| '-' expr								 # NegateExpression
    | '(' expr ')'                           # BracketedExpression
    | expr ('*' | '/') expr                  # MulDivExpression
    | expr ('+' | '-') expr                  # AddSubExpression
    | expr ('<' | '>' | '<=' | '>=') expr    # ComparisonExpression
    | expr ('=' | '!=') expr                 # EqualityExpression
    | expr '&' expr                          # AndExpression
    | expr '|' expr                          # OrExpression
    ;

ifexpr: 'If(' expr ',' expr ',' expr ')';
function: IDENTIFIER '()'
        | IDENTIFIER '(' expr (',' expr )* ')'
        ;

literal: NUMBER | STRING | DATE | TRUE | FALSE;

field: IDENTIFIER;

NUMBER: [0-9]+('.' [0-9]+)?;
STRING: '"'(~'"' | '""')*'"';
DATE: '\''[0-9][0-9][0-9][0-9] '-' [0-9][0-9] '-' [0-9][0-9] '\'';
TRUE: 'true';
FALSE: 'false';
IDENTIFIER: [a-zA-Z_$][a-zA-Z0-9_$]*;
WHITESPACE: [ \t\r\n] -> skip;
