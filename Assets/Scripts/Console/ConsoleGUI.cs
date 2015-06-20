using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts.Console
{
    public class ConsoleGUI : MonoBehaviour
    {
        private enum ConsoleDrawMode
        {
            Log,
            Help
        }

        public float AutocompleteWindowHeight = 300.0f;

        private ConsoleDrawMode drawMode = ConsoleDrawMode.Log;

        private Rect consoleWindowRect = new Rect (0, 0, 640, 480);
        private string inputText = "";
        private string lastInputText = "";

        public bool DrawConsole = false;
        private bool autocompleteWindowIsOn;
        private Vector2 logScrollPos = Vector2.zero;
        private Vector2 helpScrollPos = Vector2.zero;
        private Vector2 autocompleteScrollPos = Vector2.zero;

        public delegate void ConsoleVoidDelegate (object sender, EventArgs e);

        public static event ConsoleVoidDelegate OnConsoleOpen;
        public static event ConsoleVoidDelegate OnConsoleClose;

        private bool pendingFocusSet;

        private bool tabPressed;
        private bool escPressed;
        private bool f1Pressed;

        private readonly List<string> lastExecutedCommads = new List<string> ();
        private int currentExecutedCommandNum;

        //Regex regularExpression = new Regex("^[a-zA-Z0-9_]*$");
        private readonly Regex regularExpression = new Regex ("");
        private readonly char[] newLine = "\n\r".ToCharArray ();

        /*Regular expression, contains only upper and lowercase letters, numbers, and underscores.
 
         * ^ : start of string
        [ : beginning of character group
        a-z : any lowercase letter
        A-Z : any uppercase letter
        0-9 : any digit
        _ : underscore
        ] : end of character group
        * : zero or more of the given characters
        $ : end of string
 
    */


        private string[] currentAutocompleteResult;
        private GUIStyle logBackgroundStyle;
        private bool prevCursorState;

        // ReSharper disable once UnusedMember.Local
        private void Start ()
        {
            consoleWindowRect.x = (Screen.width - consoleWindowRect.width) * 0.5f;
            consoleWindowRect.y = (Screen.height - consoleWindowRect.height) * 0.5f;
            prevCursorState = Cursor.visible;
        }

        // ReSharper disable once UnusedMember.Local
        private void OnGUI ()
        {
            InitStyles ();
            ProcessEvents ();

            if (DrawConsole)
            {
                consoleWindowRect = GUI.Window (0, consoleWindowRect, ConsoleWindowFunc, "Console");
            }
        }

        private void ExecuteCommand (string command)
        {
            if (!lastExecutedCommads.Contains (command))
            {
                lastExecutedCommads.Add (command);
            }
            Console.Execute (command);
            inputText = "";
        }

        private void ProcessEvents ()
        {
            if (Event.current != null)
            {
                Event e = Event.current;

                if (e.isKey)
                {
                    bool keyDown = e.type == EventType.KeyDown;
                    if (keyDown && e.character == '\n')
                    {
                        ExecuteCommand (inputText);
                    }
                    switch (e.keyCode)
                    {
                        case KeyCode.DownArrow:
                            if (lastExecutedCommads != null && lastExecutedCommads.Count > 0)
                            {
                                currentExecutedCommandNum++;
                                if (currentExecutedCommandNum >= lastExecutedCommads.Count)
                                {
                                    currentExecutedCommandNum = 0;
                                }

                                inputText = lastExecutedCommads[currentExecutedCommandNum];
                                MoveCursorToEnd (inputText);
                            }

                            break;

                        case KeyCode.UpArrow:
                            if (lastExecutedCommads != null && lastExecutedCommads.Count > 0)
                            {
                                currentExecutedCommandNum--;
                                if (currentExecutedCommandNum < 0)
                                {
                                    currentExecutedCommandNum = lastExecutedCommads.Count - 1;
                                }

                                inputText = lastExecutedCommads[currentExecutedCommandNum];
                                MoveCursorToEnd (inputText);
                            }

                            break;

                        case KeyCode.Return:

                            break;

                        case KeyCode.F1:
                            if (f1Pressed == false)
                            {
                                if (keyDown)
                                {
                                    f1Pressed = true;
                                }

                                DrawConsole = !DrawConsole;

                                if (DrawConsole)
                                {
                                    prevCursorState = Cursor.visible;
                                    Cursor.visible = true;
                                    pendingFocusSet = true;
                                    if (OnConsoleOpen != null)
                                    {
                                        OnConsoleOpen (this, EventArgs.Empty);
                                    }
                                }
                                else
                                {
                                    Cursor.visible = prevCursorState;
                                    pendingFocusSet = false;
                                    if (OnConsoleClose != null)
                                    {
                                        OnConsoleClose(this, EventArgs.Empty);
                                    }
                                }
                            }
                            else if (!keyDown)
                            {
                                f1Pressed = false;
                            }

                            break;

                        case KeyCode.Escape:
                            if (escPressed == false)
                            {
                                if (keyDown)
                                {
                                    escPressed = true;
                                }

                                if (inputText.Length > 0)
                                {
                                    inputText = "";
                                }
                                else
                                {
                                    Cursor.visible = prevCursorState;
                                    DrawConsole = false;

                                    if (DrawConsole)
                                    {
                                        if (OnConsoleOpen != null)
                                        {
                                            OnConsoleOpen(this, EventArgs.Empty);
                                        }
                                    }
                                    else
                                    {
                                        if (OnConsoleClose != null)
                                        {
                                            OnConsoleClose(this, EventArgs.Empty);
                                        }
                                    }
                                }
                            }
                            else if (keyDown == false)
                            {
                                escPressed = false;
                            }

                            break;

                        case KeyCode.Tab:
                            if (keyDown)
                            {
                                if (!tabPressed)
                                {
                                    tabPressed = true;
                                    if (currentAutocompleteResult != null && currentAutocompleteResult.Length > 0)
                                    {
                                        inputText = currentAutocompleteResult[0];

                                        MoveCursorToEnd (inputText);
                                    }
                                }
                            }
                            else
                            {
                                tabPressed = false;
                            }
                            break;
                    }
                }
            }
        }

        private void MoveCursorToEnd (string text)
        {
            TextEditor editor = (TextEditor) GUIUtility.GetStateObject (typeof (TextEditor), GUIUtility.keyboardControl);
            editor.pos = text.Length;
            editor.selectPos = text.Length;
        }

        private bool DrawAutoCompleteWindow (Rect rect)
        {
            if (string.IsNullOrEmpty (inputText))
            {
                return false;
            }

            if (lastInputText != inputText)
            {
                string input = inputText.Split (' ')[0];
                currentAutocompleteResult = Console.GetAutocompleteResult (input);
            }

            if (currentAutocompleteResult == null || currentAutocompleteResult.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < 6; i++)
            {
                GUI.Box (rect, "");
            }

            GUILayout.BeginArea (rect);
            autocompleteScrollPos = GUILayout.BeginScrollView (autocompleteScrollPos);

            foreach (string result in currentAutocompleteResult)
            {
                Dictionary<string, Type> pars = Console.GetCommandParameters (result);

                GUILayout.BeginHorizontal ();
                GUILayout.Label (result, GUILayout.Width (rect.width * 0.45f));
                Color defColor = GUI.color;

                string paramStr = "";
                int index = 0;
                int keyCount = pars.Count;
                foreach (string key in pars.Keys)
                {
                    paramStr += pars[key].Name + " " + key + (index < keyCount - 1 ? ", " : "");
                }

                GUI.color = Color.yellow;
                GUILayout.Label (paramStr, GUILayout.Width (rect.width * 0.45f));
                GUI.color = defColor;

                GUILayout.EndHorizontal ();
            }

            GUILayout.EndScrollView ();
            GUILayout.EndArea ();

            return true;
        }

        private int lastLogLen;

        private void DrawLog (Rect rect)
        {
            GUI.Box (rect, "");
            GUILayout.BeginArea (rect);

            if (!autocompleteWindowIsOn)
            {
                logScrollPos = GUILayout.BeginScrollView (logScrollPos);
            }

            foreach (string logString in Console.LogStrings)
            {
                GUILayout.Label (logString);
            }

            if (!autocompleteWindowIsOn)
            {
                GUILayout.EndScrollView ();
            }

            GUILayout.EndArea ();

            if (lastLogLen != Console.LogStrings.Count)
            {
                lastLogLen = Console.LogStrings.Count;
                logScrollPos.y = rect.height;
            }
        }

        private void DrawHelp (Rect rect)
        {
            GUI.Box (rect, "");
            GUILayout.BeginArea (rect);
            helpScrollPos = GUILayout.BeginScrollView (helpScrollPos);

            GUILayout.Label ("Помощь");
            GUILayout.Space (50.0f);
            GUILayout.Label ("Клавиши управления:");
            GUILayout.Space (20.0f);
            GUILayout.Label ("F1 - открыть/скрыть консоль");
            GUILayout.Label ("Enter - выполнить текущую команду");
            GUILayout.Label ("Tab - автоматическое завершение наиболее подходящей команды");
            GUILayout.Label (
                        "Esc - Если в поле комманд введен текст, нажатие удаляет его. Если поле ввода команд пусто - нажатие закрывает окно консоли");
            GUILayout.Label ("Стрелки Вверх и Вниз - выбор последних введенных команд");

            GUILayout.EndScrollView ();
            GUILayout.EndArea ();
        }

        private IEnumerator ChangeAutocompleteWindowStateFlag (bool on)
        {
            yield return new WaitForEndOfFrame ();
            autocompleteWindowIsOn = on;
        }

        private void ConsoleWindowFunc (int windowId)
        {
            Rect tempRect = new Rect (consoleWindowRect);

            tempRect.x = 0;
            tempRect.y = 0;

            GUI.Box (tempRect, "", logBackgroundStyle);

            float padding = 5;
            float topYpadding = 20;
            float inputH = 25;
            float toolbarW = 100;
            float logW = consoleWindowRect.width - padding * 2 - toolbarW; // *0.7f - ;
            float logH = consoleWindowRect.height - (topYpadding + inputH + padding);


            // main
            tempRect.x = padding;
            tempRect.y = topYpadding;
            tempRect.width = Mathf.Max (0, logW);
            tempRect.height = Mathf.Max (0, logH);

            switch (drawMode)
            {
                case ConsoleDrawMode.Log:
                    DrawLog (tempRect);
                    break;

                case ConsoleDrawMode.Help:
                    DrawHelp (tempRect);
                    break;
            }


            // INPUT
            tempRect.y += tempRect.height;
            tempRect.height = inputH;


            if (currentAutocompleteResult != null && currentAutocompleteResult.Length > 0)
            {
                string match = currentAutocompleteResult[0].ToLower ();
                if (match.StartsWith (inputText.ToLower ()))
                {
                    string subs = currentAutocompleteResult[0];
                    subs = subs.Remove (0, inputText.Length);
                    subs = inputText + subs;
                    tempRect.x += 3;
                    GUI.Label (tempRect, subs);
                    tempRect.x -= 3;
                }
            }

            GUI.SetNextControlName ("Input");
            string temp = GUI.TextField (tempRect, inputText);

            if (pendingFocusSet)
            {
                pendingFocusSet = false;
                GUI.FocusControl ("Input");
            }

            if (regularExpression.IsMatch (temp))
            {
                if (temp.IndexOfAny (newLine) != -1)
                {
                    ExecuteCommand (inputText);
                }

                else
                {
                    inputText = temp;
                }
            }
            else
            {
                Debug.Log ("Wrong string");
            }

            // AUTOCOMPLETE
            tempRect.width = logW;
            tempRect.height = AutocompleteWindowHeight;
            tempRect.y -= tempRect.height;

            bool tmpFlag = DrawAutoCompleteWindow (tempRect);
            if (tmpFlag != autocompleteWindowIsOn)
            {
                StopCoroutine ("changeAutocompleteWindowStateFlag");
                StartCoroutine (ChangeAutocompleteWindowStateFlag (tmpFlag));
            }


            // TOOLBAR
            tempRect.x = padding + logW;
            tempRect.y = topYpadding;
            tempRect.height = logH;
            tempRect.width = toolbarW;

            GUI.Box (tempRect, "");

            GUILayout.BeginArea (tempRect);

            string[] toolbarContent =
            {
                "Лог", "Помощь"
            };
            drawMode = (ConsoleDrawMode) GUILayout.SelectionGrid ((int) drawMode, toolbarContent, 1);

            GUILayout.EndArea ();

            // EXEC
            tempRect.x = padding + logW;
            tempRect.y = logH + topYpadding;
            tempRect.height = inputH;
            tempRect.width = toolbarW;

            if (GUI.Button (tempRect, "Выполнить"))
            {
                ExecuteCommand (inputText);
            }

            GUI.DragWindow (new Rect (0, 0, 10000, 10000));

            if (lastInputText != inputText)
            {
                lastInputText = inputText;
                autocompleteScrollPos = Vector2.zero;
            }
        }

        private void InitStyles ()
        {
            if (logBackgroundStyle == null)
            {
                logBackgroundStyle = new GUIStyle (GUI.skin.box);
                logBackgroundStyle.normal.background = MakeTex (16, 16, new Color (0f, 0.1f, 0.1f, 1f));
            }
        }

        private Texture2D MakeTex (int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D (width, height);
            result.SetPixels (pix);
            result.Apply ();
            return result;
        }
    }
}