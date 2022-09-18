using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Calculator.StateManagement
{
    /// <summary>
    /// See <see href="https://www.figma.com/file/ErCYBNpRccDCIVZ6I0j3uW/Calculator?node-id=0%3A1">this</href> for more information on the states
    /// </summary>
    public enum CALCULATOR_STATE
    {
        STATE_0 = 0,
        STATE_1,
        STATE_2,
        STATE_3,
        STATE_4,
        STATE_5,
        STATE_6,
        STATE_7,
    }

    public enum OPERATOR
    {
        ADDITION,
        SUBTRACTION,
        MULTIPLICATION,
        DIVISION
    }

    public abstract class StateTransition { }

    public sealed class SelectEqualsTransition : StateTransition { }

    public sealed class SelectNumberTransition : StateTransition
    {
        public float Number;

        public SelectNumberTransition(float number)
        {
            this.Number = number;
        }
    }

    public sealed class SelectOperatorTransition : StateTransition
    {
        public OPERATOR Operator;

        public SelectOperatorTransition(string operatorString)
        {
            switch (operatorString)
            {
                case "+":
                {
                    this.Operator = OPERATOR.ADDITION;
                    break;
                }
                case "-":
                {
                    this.Operator = OPERATOR.SUBTRACTION;
                    break;
                }
                case "*":
                {
                    this.Operator = OPERATOR.MULTIPLICATION;
                    break;
                }
                case "/":
                {
                    this.Operator = OPERATOR.DIVISION;
                    break;
                }
                default:
                {
                    Debug.LogError(
                        $"Received unexpected value {operatorString} for SelectOperatorTransition"
                    );
                    break;
                }
            }
        }
    }

    public class CalculatorStateManager
    {
        /// <summary>
        /// The currently active state
        /// </summary>
        private CALCULATOR_STATE? activeState = null;

        /// <summary>
        /// The left-hand side of the operation
        /// </summary>
        private float? lhs = null;

        /// <summary>
        /// The right-hand side of the operation
        /// </summary>
        private float? rhs = null;

        private OPERATOR? _operator = null;

        /// <summary>
        /// The result of the operation
        /// </summary>
        private float? result = null;

        public delegate void OnCalculatorMessageChanged(string newMessage);
        public OnCalculatorMessageChanged onCalculatorMessageChanged;

        /// <summary>
        /// The message which should be displayed in the Calculator display pane
        /// </summary>
        public string CalculatorMessage
        {
            get
            {
                if (lhs == null)
                {
                    return $"= ";
                }
                else if (_operator == null)
                {
                    return $"= {lhs}";
                }
                else if (rhs == null)
                {
                    return $"= {lhs} {OperatorToString((OPERATOR)_operator)}";
                }
                else if (result == null)
                {
                    return $"= {lhs} {OperatorToString((OPERATOR)_operator)} {rhs}";
                }
                else
                {
                    return $"= {lhs} {OperatorToString((OPERATOR)_operator)} {rhs} = {result}";
                }
            }
        }

        /// <summary>
        /// The result of the operation performed by the calculator
        /// </summary>
        public float? Result
        {
            get => result;
        }

        public CalculatorStateManager()
        {
            this.activeState = CALCULATOR_STATE.STATE_0;
            this.lhs = 0;
        }

        private class StateTransitionArguments
        {
            public CALCULATOR_STATE from;
            public CALCULATOR_STATE to;
            public Type transition;
        }

        private List<StateTransitionArguments> allTransitions = new List<StateTransitionArguments>()
        {
            //STATE 0
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_0,
                to = CALCULATOR_STATE.STATE_1,
                transition = typeof(SelectNumberTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_0,
                to = CALCULATOR_STATE.STATE_7,
                transition = typeof(SelectEqualsTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_0,
                to = CALCULATOR_STATE.STATE_3,
                transition = typeof(SelectOperatorTransition)
            },
            //STATE 1
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_1,
                to = CALCULATOR_STATE.STATE_2,
                transition = typeof(SelectNumberTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_1,
                to = CALCULATOR_STATE.STATE_7,
                transition = typeof(SelectEqualsTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_1,
                to = CALCULATOR_STATE.STATE_3,
                transition = typeof(SelectOperatorTransition)
            },
            //STATE 2
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_2,
                to = CALCULATOR_STATE.STATE_2,
                transition = typeof(SelectNumberTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_2,
                to = CALCULATOR_STATE.STATE_7,
                transition = typeof(SelectEqualsTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_2,
                to = CALCULATOR_STATE.STATE_3,
                transition = typeof(SelectOperatorTransition)
            },
            //STATE 3
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_3,
                to = CALCULATOR_STATE.STATE_4,
                transition = typeof(SelectNumberTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_3,
                to = CALCULATOR_STATE.STATE_7,
                transition = typeof(SelectEqualsTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_3,
                to = CALCULATOR_STATE.STATE_3,
                transition = typeof(SelectOperatorTransition)
            },
            //STATE 4
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_4,
                to = CALCULATOR_STATE.STATE_5,
                transition = typeof(SelectNumberTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_4,
                to = CALCULATOR_STATE.STATE_6,
                transition = typeof(SelectEqualsTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_4,
                to = CALCULATOR_STATE.STATE_3,
                transition = typeof(SelectOperatorTransition)
            },
            //STATE 5
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_5,
                to = CALCULATOR_STATE.STATE_5,
                transition = typeof(SelectNumberTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_5,
                to = CALCULATOR_STATE.STATE_6,
                transition = typeof(SelectEqualsTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_5,
                to = CALCULATOR_STATE.STATE_3,
                transition = typeof(SelectOperatorTransition)
            },
            //STATE 6
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_6,
                to = CALCULATOR_STATE.STATE_1,
                transition = typeof(SelectNumberTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_6,
                to = CALCULATOR_STATE.STATE_6,
                transition = typeof(SelectEqualsTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_6,
                to = CALCULATOR_STATE.STATE_3,
                transition = typeof(SelectOperatorTransition)
            },
            //STATE 7
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_7,
                to = CALCULATOR_STATE.STATE_1,
                transition = typeof(SelectNumberTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_7,
                to = CALCULATOR_STATE.STATE_7,
                transition = typeof(SelectEqualsTransition)
            },
            new StateTransitionArguments()
            {
                from = CALCULATOR_STATE.STATE_7,
                to = CALCULATOR_STATE.STATE_3,
                transition = typeof(SelectOperatorTransition)
            },
        };

        private void HandleTransition(StateTransition transition)
        {
            var currentTransition = allTransitions.Find(
                t => t.from == this.activeState && transition.GetType() == t.transition
            );
            Debug.Log(
                $"Transitioning from {currentTransition.from} to {currentTransition.to} given transition {transition.GetType()}"
            );

            if (transition is SelectNumberTransition)
            {
                var number = ((SelectNumberTransition)transition).Number;
                switch (currentTransition.from)
                {
                    case CALCULATOR_STATE.STATE_0:
                    {
                        this.lhs = number;
                        ResetAllButNum1();
                        break;
                    }

                    case CALCULATOR_STATE.STATE_1:
                    case CALCULATOR_STATE.STATE_2:
                    {
                        this.lhs = float.Parse($"{this.lhs}{number}");
                        this.ResetAllButNum1();
                        break;
                    }
                    case CALCULATOR_STATE.STATE_3:
                    {
                        this.rhs = number;
                        this.result = null;
                        break;
                    }
                    case CALCULATOR_STATE.STATE_4:
                    case CALCULATOR_STATE.STATE_5:
                    {
                        this.rhs = float.Parse($"{this.rhs}{number}");
                        this.result = null;
                        break;
                    }
                    case CALCULATOR_STATE.STATE_6:
                    case CALCULATOR_STATE.STATE_7:
                    {
                        this.lhs = number;
                        this.ResetAllButNum1();
                        break;
                    }
                }
            }
            else if (transition is SelectOperatorTransition)
            {
                var __operator = ((SelectOperatorTransition)transition).Operator;
                switch (currentTransition.from)
                {
                    case CALCULATOR_STATE.STATE_0:
                    {
                        this._operator = __operator;
                        break;
                    }
                    case CALCULATOR_STATE.STATE_1:
                    case CALCULATOR_STATE.STATE_2:
                    case CALCULATOR_STATE.STATE_3:
                    {
                        this._operator = __operator;
                        this.rhs = null;
                        break;
                    }
                    case CALCULATOR_STATE.STATE_4:
                    case CALCULATOR_STATE.STATE_5:
                    case CALCULATOR_STATE.STATE_6:
                    case CALCULATOR_STATE.STATE_7:
                    {
                        if (this._operator != null && this.rhs != null)
                        {
                            this.lhs = FindResult(this.lhs, this._operator, this.rhs);
                        }

                        this._operator = __operator;
                        this.rhs = null;
                        break;
                    }
                }
            }
            else if (transition is SelectEqualsTransition)
            {
                switch (currentTransition.from)
                {
                    case CALCULATOR_STATE.STATE_0:
                    case CALCULATOR_STATE.STATE_1:
                    case CALCULATOR_STATE.STATE_2:
                    case CALCULATOR_STATE.STATE_3:
                    case CALCULATOR_STATE.STATE_7:
                    {
                        this.result = this.lhs;
                        break;
                    }
                    case CALCULATOR_STATE.STATE_4:
                    case CALCULATOR_STATE.STATE_5:
                    case CALCULATOR_STATE.STATE_6:
                    {
                        this.result = this.FindResult(this.lhs, this._operator, this.rhs);
                        break;
                    }
                }
            }
            this.activeState = currentTransition.to;
            this.LogState();
            if (this.onCalculatorMessageChanged != null)
            {
                this.onCalculatorMessageChanged.Invoke(CalculatorMessage);
            }
        }

        /// <summary>
        /// Given our num1 and num2, and given our enum operator, return the float value
        /// </summary>
        private float FindResult(float? num1, OPERATOR? _operator, float? num2)
        {
            if (num1 == null)
            {
                Debug.LogError("FindResult is called when num1 is null!");
            }
            if (num2 == null)
            {
                Debug.LogError("FindResult is called when num2 is null!");
            }
            switch (_operator)
            {
                case OPERATOR.ADDITION:
                {
                    return (float)num1 + (float)num2;
                }
                case OPERATOR.SUBTRACTION:
                {
                    return (float)num1 - (float)num2;
                }
                case OPERATOR.MULTIPLICATION:
                {
                    return (float)num1 * (float)num2;
                }
                case OPERATOR.DIVISION:
                {
                    return (float)num1 / (float)num2;
                }
                default:
                {
                    Debug.LogError($"Received invalid value {_operator} for operator");
                    return -1;
                }
            }
        }

        private void ResetAllButNum1()
        {
            this.rhs = null;
            this._operator = null;
            this.result = null;
        }

        public void SelectNumber(float number)
        {
            this.HandleTransition(new SelectNumberTransition(number));
        }

        public void SelectOperator(string operatorString)
        {
            this.HandleTransition(new SelectOperatorTransition(operatorString));
        }

        public void SelectEquals()
        {
            this.HandleTransition(new SelectEqualsTransition());
        }

        private void LogState()
        {
            Debug.Log($"num1: {lhs}\toperator: {this._operator}\tnum2: {rhs}\tresult: {result}");
        }

        private static string OperatorToString(OPERATOR o)
        {
            switch (o)
            {
                case OPERATOR.ADDITION:
                    return "+";
                case OPERATOR.SUBTRACTION:
                    return "-";
                case OPERATOR.MULTIPLICATION:
                    return "*";
                case OPERATOR.DIVISION:
                default:
                    return "/";
            }
        }
    }
}
