using UnityEngine;
using UnityEngine.UI;
using TexDrawLib;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class TexSampleTryPractice : MonoBehaviour
{

    public InputField input;
    public TEXDraw suggestTxt;
    public TEXDraw benchmarkTxt;
    public Text warningText;
    public GameObject warningBox;
    public TEXDraw tex;
    public Dropdown dropdown;
    [Space]
    public string[] templates;
    public string[] templateDisplays;
    public int[] sizeOps;
    public int curSize;
    public int customFont;
    public bool rtlMode;

    const string emptySuggestion =
        @"Type a backslash '\\' and suggestions for commands and symbol will come here";

    const string startSuggestion = 
        @"You just type a backslash. Unfornatunely you need to type atleast a character more since there are 600+ symbols and 50+ possible commands in TEXDraw";
    
    // When input text gets changed ...
    public void InputUpdate()
    {
    //    dropdown.value = System.Array.IndexOf(templates, input.text);
        //Standard Update....
        tex.text = input.text;

        //Go find some suggestions...
        string typed = DetectTypedSymbol(input.text, input.caretPosition);
        string suggest;
        if (string.IsNullOrEmpty(typed))
            suggest = emptySuggestion;
        else if (typed == "\\")
            suggest = startSuggestion;
        else
            suggest = GetPossibleSymbols(typed.Substring(1));
        suggestTxt.text = suggest;
        DoBenchmark();
    }
    
    void Start ()
    {
        var op = dropdown.options;
        op.Clear();
        op.Add(new Dropdown.OptionData("Custom (Use Here for Template)"));
        for (int i = 0; i < templateDisplays.Length; i++)
        {
            op.Add(new Dropdown.OptionData(templateDisplays[i]));
        }
        dropdown.value = 0;
    }

    void Update()
    {
        if (!tex.preference)
            Debug.LogError("REAL NULL!!");
        //for warning informations, it can't be updated instantly
        //since changes happen only when repainting call
        warningBox.SetActive(tex.debugReport != string.Empty);
        warningText.text = tex.debugReport;
        if(!string.IsNullOrEmpty(tex.debugReport))
        {
          suggestTxt.text = tex.debugReport;
        }
    }

	static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
	//static float lastBenchmark;
    void DoBenchmark ()
    {
		stopwatch.Reset();
    	stopwatch.Start();
		
    	tex.drawingContext.Parse(tex.text);
    	tex.drawingContext.BoxPacking(tex.drawingParams);
    	tex.drawingContext.Render(tex.m_mesh, tex.drawingParams);

    	stopwatch.Stop();

    	//var end = () * 1000;

    	benchmarkTxt.text = "\\text[i]{" + stopwatch.Elapsed.TotalMilliseconds.ToString("0.00") + " ms}";
    }

    public string DetectTypedSymbol(string full, int caretPos)
    {
        string watchedStr = input.text.Substring(0, input.caretPosition);
        return Regex.Match(watchedStr, @"\\[\w]*$").Value;
    }

    List<string> keys = ListPool<string>.Get();
   
    public string GetPossibleSymbols(string raw)
    {
        keys.Clear();
        var arr = tex.preference.symbols.Keys;
        var arr2 = TexFormulaParser.commands;
        var repRaw = "\\style[b]{" + raw + "}";
        foreach (var item in arr2)
        {
            if (item.Contains(raw))
                keys.Add("\\rhold[3]\\color[yellow]" + item.Replace(raw, repRaw) + "}");
        }
        if (keys.Count > 0)
            keys.Add("----------");
        foreach (var item in arr)
        {
            if (item.Contains(raw))
				keys.Add("\\rhold[3]{ \\" + item + " }" + item.Replace(raw, repRaw));
        }
        
        return string.Join("\n", keys.ToArray());
    }


    public void UpdateAlignment(int alignment)
    {
        tex.alignment = new Vector2(alignment / 2f, 0.5f);
    }

    public void AlignmentLeft(bool yes)
    {
        if(yes)
            UpdateAlignment(0);
    }

    public void AlignmentCenter(bool yes)
    {
        if(yes)
            UpdateAlignment(1);
    }

    public void AlignmentRight(bool yes)
    {
        if(yes)
            UpdateAlignment(2);
    }

    public void UpdateWrap(int wrap)
    {
        tex.autoWrap = (Wrapping)(wrap + (rtlMode ? 4 : 0));
    }
    
    public void UpdateTemplate(int idx)
    {
        if (idx > 0)
            input.text = TEXSupNewLine.Replace(templates[idx - 1]);
    }
    
    public void UpdateCustomFont(bool custom)
    {
        tex.fontIndex = custom ? customFont : -1;
    }
    
    public void UpdateSize(bool larger)
    {
        curSize = Mathf.Clamp(curSize + (larger ? 1 : -1), 0, sizeOps.Length);
        tex.size = sizeOps[curSize];
    }

    public void UpdateRTL(bool RTL)
    {
    	rtlMode = RTL;
		tex.autoWrap = (Wrapping)((int)tex.autoWrap % 4 + (rtlMode ? 4 : 0));
		tex.GetComponent<TEXSupRTLSupport>().enabled = RTL;
    }

    #if UNITY_EDITOR
    [ContextMenu("Export")]
    void Export ()
    {
    	var str = string.Join("\n", templates);
    	GUIUtility.systemCopyBuffer = str;
    }
	[ContextMenu("Import")]
    void Import ()
    {
    	UnityEditor.Undo.RecordObject(this, "Importing");
		templates = GUIUtility.systemCopyBuffer.Split('\n');
    }
    #endif
}
