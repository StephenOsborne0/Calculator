using System;
using Shared.Models.Parser;
using Shared.Models.Parser.Exceptions;
using Shared.Models.Parser.OperationNodes;

namespace ScriptEngine.Business.Operations
{
    internal class Operations
    {
        private static bool TryCast<T>(object obj, out T result) where T : struct
        {
            try
            {
                result = (T)Convert.ChangeType(obj, typeof(T));
                return true;
            }
            catch (Exception ex)
            {
                result = default;
                return false;
            }
        }

        internal static object Addition(AdditionNode additionNode)
        {
            var lhsOperand = additionNode.Lhs.Evaluate();
            var rhsOperand = additionNode.Rhs.Evaluate();

            if (TryCast<double>(lhsOperand, out var doubleA) && 
                TryCast<double>(rhsOperand, out var doubleB))
                return doubleA + doubleB;

            if (TryCast<bool>(lhsOperand, out var boolA) && 
                TryCast<bool>(rhsOperand, out var boolB))
                return Convert.ToBoolean(Convert.ToInt32(boolA) + Convert.ToInt32(boolB));

            return string.Concat(lhsOperand.ToString(), rhsOperand.ToString());
        }

        internal static object Subtraction(SubtractionNode subtractionNode)
        {
            var lhsOperand = subtractionNode.Lhs.Evaluate();
            var rhsOperand = subtractionNode.Rhs.Evaluate();

            if (TryCast<double>(lhsOperand, out var doubleA) && 
                TryCast<double>(rhsOperand, out var doubleB))
                return doubleA - doubleB;

            if (TryCast<bool>(lhsOperand, out var boolA) &&
                TryCast<bool>(rhsOperand, out var boolB))
                return Convert.ToBoolean(Convert.ToInt32(boolA) - Convert.ToInt32(boolB));

            return lhsOperand.ToString().Replace(rhsOperand.ToString(), "");
        }

        internal static object Multiplication(MultiplicationNode multiplicationNode)
        {
            var lhsOperand = multiplicationNode.Lhs.Evaluate();
            var rhsOperand = multiplicationNode.Rhs.Evaluate();

            if (TryCast<double>(lhsOperand, out var doubleA) && 
                TryCast<double>(rhsOperand, out var doubleB))
                return doubleA * doubleB;

            if (TryCast<bool>(lhsOperand, out var boolA) &&
                TryCast<bool>(rhsOperand, out var boolB))
                return Convert.ToBoolean(Convert.ToInt32(boolA) * Convert.ToInt32(boolB));

            if (TryCast<double>(rhsOperand, out var doubleC))
                return string.Join(lhsOperand.ToString(), doubleC);
            if (TryCast<double>(lhsOperand, out var doubleD))
                return string.Join(rhsOperand.ToString(), doubleD);

            throw new OperationException("Multiplication", lhsOperand, rhsOperand);
        }

        internal static object Division(DivisionNode divisionNode)
        {
            var lhsOperand = divisionNode.Lhs.Evaluate();
            var rhsOperand = divisionNode.Rhs.Evaluate();

            if (TryCast<double>(lhsOperand, out var doubleA) && 
                TryCast<double>(rhsOperand, out var doubleB))
            {
                if (doubleB == 0)
                    throw new DivideByZeroException();
                return doubleA / doubleB;
            }

            throw new OperationException("Division", lhsOperand, rhsOperand);
        }

        internal static object Modulus(ModulusNode modulusNode)
        {
            var lhsOperand = modulusNode.Lhs.Evaluate();
            var rhsOperand = modulusNode.Rhs.Evaluate();

            if (TryCast<double>(lhsOperand, out var doubleA) && 
                TryCast<double>(rhsOperand, out var doubleB))
                return doubleA % doubleB;

            throw new OperationException("Division", lhsOperand, rhsOperand);
        }

        internal static object Bitshift(BitshiftNode bitshiftNode)
        {
            var lhsOperand = bitshiftNode.Lhs.Evaluate();
            var rhsOperand = bitshiftNode.Rhs.Evaluate();

            if (TryCast<long>(lhsOperand, out var longA) &&
                TryCast<int>(rhsOperand, out var intB))
            {
                switch (bitshiftNode.Value.ToString())
                {
                    case CharacterSet.BITSHIFT_RIGHT:
                        return longA >> intB;
                    case CharacterSet.BITSHIFT_LEFT:
                        return longA << intB;
                }
            }

            if (TryCast<int>(rhsOperand, out var intC))
            {
                var strA = lhsOperand.ToString();

                switch (bitshiftNode.Value.ToString())
                {
                    case CharacterSet.BITSHIFT_RIGHT:
                        return strA.Substring(strA.Length - (intC % strA.Length)) + 
                               strA.Substring(0, strA.Length - (intC % strA.Length));
                    case CharacterSet.BITSHIFT_LEFT:
                        return strA.Substring(intC % strA.Length) + 
                               strA.Substring(0, intC % strA.Length);
                }
            }

            throw new OperationException("Bitshift", lhsOperand, rhsOperand);
        }

        internal static object Comparison(ComparisonNode comparisonNode)
        {
            var lhsOperand = comparisonNode.Lhs.Evaluate();
            var rhsOperand = comparisonNode.Rhs.Evaluate();

            if (TryCast<double>(lhsOperand, out var doubleA) && 
                TryCast<double>(rhsOperand, out var doubleB))
            {
                switch (comparisonNode.Value.ToString())
                {
                    case CharacterSet.LESS_THAN:
                        return doubleA < doubleB;
                    case CharacterSet.GREATER_THAN:
                        return doubleA > doubleB;
                    case CharacterSet.LESS_THAN_EQUAL:
                        return doubleA <= doubleB;
                    case CharacterSet.GREATER_THAN_EQUAL:
                        return doubleA >= doubleB;
                }
            }

            switch (comparisonNode.Value.ToString())
            {
                case CharacterSet.LESS_THAN:
                    return lhsOperand.ToString().Length < rhsOperand.ToString().Length;
                case CharacterSet.GREATER_THAN:
                    return lhsOperand.ToString().Length > rhsOperand.ToString().Length;
                case CharacterSet.LESS_THAN_EQUAL:
                    return lhsOperand.ToString().Length <= rhsOperand.ToString().Length;
                case CharacterSet.GREATER_THAN_EQUAL:
                    return lhsOperand.ToString().Length >= rhsOperand.ToString().Length;
            }

            throw new OperationException("Comparison", lhsOperand, rhsOperand);
        }

        internal static object Equality(EqualityNode equalityNode)
        {
            var lhsOperand = equalityNode.Lhs.Evaluate();
            var rhsOperand = equalityNode.Rhs.Evaluate();

            if (TryCast<double>(lhsOperand, out var doubleA) && 
                TryCast<double>(rhsOperand, out var doubleB))
            {
                switch (equalityNode.Value.ToString())
                {
                    case CharacterSet.EQUALITY:
                        return doubleA == doubleB;
                    case CharacterSet.NOT_EQUAL:
                    case CharacterSet.NOT_EQUAL2:
                        return doubleA != doubleB;
                }
            }

            if (TryCast<bool>(lhsOperand, out var boolA) && 
                TryCast<bool>(rhsOperand, out var boolB))
            {
                switch (equalityNode.Value.ToString())
                {
                    case CharacterSet.EQUALITY:
                        return boolA == boolB;
                    case CharacterSet.NOT_EQUAL:
                    case CharacterSet.NOT_EQUAL2:
                        return boolA != boolB;
                }
            }

            switch (equalityNode.Value.ToString())
            {
                case CharacterSet.EQUALITY:
                    return lhsOperand.ToString() == rhsOperand.ToString();
                case CharacterSet.NOT_EQUAL:
                case CharacterSet.NOT_EQUAL2:
                    return lhsOperand.ToString() != rhsOperand.ToString();
            }

            throw new OperationException("Equality", lhsOperand, rhsOperand);
        }

        internal static object BitwiseAnd(BitwiseAndNode bitwiseAndNode)
        {
            var lhsOperand = bitwiseAndNode.Lhs.Evaluate();
            var rhsOperand = bitwiseAndNode.Rhs.Evaluate();

            if (TryCast<long>(lhsOperand, out var longA) && 
                TryCast<long>(rhsOperand, out var longB) && 
                bitwiseAndNode.Value.ToString() == CharacterSet.BITWISE_AND)
                return longA & longB;

            if (TryCast<bool>(lhsOperand, out var boolA) && 
                TryCast<bool>(rhsOperand, out var boolB) && 
                bitwiseAndNode.Value.ToString() == CharacterSet.BITWISE_AND)
                return boolA & boolB;

            throw new OperationException("Bitwise And", lhsOperand, rhsOperand);
        }

        internal static object BitwiseXor(BitwiseXorNode bitwiseXorNode)
        {
            var lhsOperand = bitwiseXorNode.Lhs.Evaluate();
            var rhsOperand = bitwiseXorNode.Rhs.Evaluate();

            if (TryCast<long>(lhsOperand, out var longA) && 
                TryCast<long>(rhsOperand, out var longB) && 
                bitwiseXorNode.Value.ToString() == CharacterSet.BITWISE_XOR)
                return longA ^ longB;

            if (TryCast<bool>(lhsOperand, out var boolA) && 
                TryCast<bool>(rhsOperand, out var boolB) && 
                bitwiseXorNode.Value.ToString() == CharacterSet.BITWISE_XOR)
                return boolA ^ boolB;

            throw new OperationException("Bitwise Xor", lhsOperand, rhsOperand);
        }

        internal static object BitwiseOr(BitwiseOrNode bitwiseOrNode)
        {
            var lhsOperand = bitwiseOrNode.Lhs.Evaluate();
            var rhsOperand = bitwiseOrNode.Rhs.Evaluate();

            if (TryCast<long>(lhsOperand, out var longA) &&
                TryCast<long>(rhsOperand, out var longB) &&
                bitwiseOrNode.Value.ToString() == CharacterSet.BITWISE_OR)
                return longA | longB;

            if (TryCast<bool>(lhsOperand, out var boolA) &&
                TryCast<bool>(rhsOperand, out var boolB) &&
                bitwiseOrNode.Value.ToString() == CharacterSet.BITWISE_OR)
                return boolA | boolB;

            throw new OperationException("Bitwise Or", lhsOperand, rhsOperand);
        }

        internal static object LogicalAnd(LogicalAndNode logicalAndNode)
        {
            var lhsOperand = logicalAndNode.Lhs.Evaluate();
            var rhsOperand = logicalAndNode.Rhs.Evaluate();

            if (TryCast<bool>(lhsOperand, out var boolA) &&
                TryCast<bool>(rhsOperand, out var boolB) &&
                logicalAndNode.Value.ToString() == CharacterSet.LOGICAL_AND)
                return boolA && boolB;

            throw new OperationException("Logical And", lhsOperand, rhsOperand);
        }

        internal static object LogicalOr(LogicalOrNode logicalOrNode)
        {
            var lhsOperand = logicalOrNode.Lhs.Evaluate();
            var rhsOperand = logicalOrNode.Rhs.Evaluate();

            if (TryCast<bool>(lhsOperand, out var boolA) &&
                TryCast<bool>(rhsOperand, out var boolB) &&
                logicalOrNode.Value.ToString() == CharacterSet.LOGICAL_OR)
                return boolA || boolB;

            throw new OperationException("Logical Or", lhsOperand, rhsOperand);
        }

        internal static object Ternary(TernaryNode ternaryNode)
        {
            var lhsOperand = ternaryNode.Lhs.Evaluate();
            var rhsOperand = ternaryNode.Rhs.Evaluate();

            if (TryCast<bool>(ternaryNode.Condition.Evaluate(), out var condition))
                return condition ? lhsOperand : rhsOperand;

            throw new OperationException("Ternary", lhsOperand, rhsOperand);
        }

        internal static object Unary(UnaryNode unaryNode)
        {
            var lhsOperand = unaryNode.Lhs.Evaluate();

            if (TryCast<double>(lhsOperand, out var doubleA))
            {
                switch (unaryNode.Value.ToString())
                {
                    case CharacterSet.ADDITION:
                        return doubleA;
                    case CharacterSet.SUBTRACTION:
                        return -doubleA;
                    case CharacterSet.SQUARE_ROOT:
                        return Math.Sqrt(doubleA);
                    case CharacterSet.SQUARE:
                        return doubleA * doubleA;
                    case CharacterSet.INCREMENT:
                        return doubleA + 1;
                    case CharacterSet.DECREMENT:
                        return doubleA - 1;
                }
            }

            if (TryCast<long>(lhsOperand, out var longA))
            {
                switch (unaryNode.Value.ToString())
                {
                    case CharacterSet.ONES_COMPLEMENT:
                        return ~longA;
                }
            }

            if (TryCast<bool>(lhsOperand, out var boolA))
            {
                switch (unaryNode.Value.ToString())
                {
                    case CharacterSet.NOT:
                        return !boolA;
                }
            }

            throw new OperationException("Unary", lhsOperand, null);
        }

        internal static object Assignment(AssignmentNode assignmentNode)
        {
            var lhsOperand = assignmentNode.Lhs.Evaluate();
            var rhsOperand = assignmentNode.Rhs.Evaluate();

            if (!(assignmentNode.Lhs is VariableNode varNode))
                throw new OperationException("Assignment", lhsOperand, rhsOperand);

            if (assignmentNode.Value.ToString() == CharacterSet.ASSIGNMENT)
            {
                varNode.Value = rhsOperand;
                return rhsOperand;
            }

            //Add string multiplication back in
            if (TryCast<double>(lhsOperand, out var doubleA) &&
                TryCast<double>(rhsOperand, out var doubleB))
            { 
                switch (assignmentNode.Value.ToString())
                {
                    case CharacterSet.ASSIGNMENT_ADDITION:
                        varNode.Value = doubleA + doubleB;
                        break;
                    case CharacterSet.ASSIGNMENT_SUBTRACTION:
                        varNode.Value = doubleA - doubleB;
                        break;
                    case CharacterSet.ASSIGNMENT_MULTIPLICATION:
                        varNode.Value = doubleA * doubleB;
                        break;
                    case CharacterSet.ASSIGNMENT_DIVISION:
                        varNode.Value = doubleA / doubleB;
                        break;
                    case CharacterSet.ASSIGNMENT_MODULUS:
                        varNode.Value = doubleA % doubleB;
                        break;
                }

                return varNode.Value;
            }

            if (TryCast<long>(lhsOperand, out var longA) &&
                TryCast<long>(rhsOperand, out var longB))
            { 
                switch (assignmentNode.Value.ToString())
                {
                    case CharacterSet.ASSIGNMENT_BITWISE_AND:
                        varNode.Value = longA & longB;
                        break;
                    case CharacterSet.ASSIGNMENT_BITWISE_OR:
                        varNode.Value = longA | longB;
                        break;
                    case CharacterSet.ASSIGNMENT_BITWISE_XOR:
                        varNode.Value = longA ^ longB;
                        break;
                }

                return varNode.Value;
            }

            if (TryCast<bool>(lhsOperand, out var boolA) &&
                TryCast<bool>(rhsOperand, out var boolB))
            {
                switch (assignmentNode.Value.ToString())
                {
                    case CharacterSet.ASSIGNMENT_BITWISE_AND:
                        varNode.Value = boolA & boolB;
                        break;
                    case CharacterSet.ASSIGNMENT_BITWISE_OR:
                        varNode.Value = boolA | boolB;
                        break;
                    case CharacterSet.ASSIGNMENT_BITWISE_XOR:
                        varNode.Value = boolA ^ boolB;
                        break;
                    case CharacterSet.ASSIGNMENT_LOGICAL_AND:
                        varNode.Value = boolA && boolB;
                        break;
                    case CharacterSet.ASSIGNMENT_LOGICAL_OR:
                        varNode.Value = boolA || boolB;
                        break;
                }

                return varNode.Value;
            }

            if (TryCast<double>(rhsOperand, out var doubleC))
            {
                switch (assignmentNode.Value.ToString())
                {
                    case CharacterSet.ASSIGNMENT_MULTIPLICATION:
                        varNode.Value = string.Join(varNode.Value.ToString(), doubleC);
                        break;
                }

                return varNode.Value;
            }

            switch (assignmentNode.Value.ToString())
            {
                case CharacterSet.ASSIGNMENT_ADDITION:
                    varNode.Value = string.Concat(varNode.Value.ToString(), rhsOperand.ToString());
                    break;
                case CharacterSet.ASSIGNMENT_SUBTRACTION:
                    varNode.Value = varNode.Value.ToString().Replace(rhsOperand.ToString(), "");
                    break;
            }

            return varNode.Value;
        }
    }
}
