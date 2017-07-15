using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResolutionDropdown : MonoBehaviour
{
    private const char ResolutionDelimiter = 'x';

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Dropdown dropdown;
#pragma warning restore 0649

	void Start()
	{
        List<Resolution> resolutions = new List<Resolution>(Screen.resolutions);
        IEnumerable<string> resolutionStrings = (from resolution
                                          in resolutions.OrderBy(a => a.width).ThenBy(a => a.height)
                                          select resolution.width.ToString() + ResolutionDelimiter + resolution.height.ToString())
                                          .Distinct();
        List<Dropdown.OptionData> dropList = (from resolution
                                              in resolutionStrings
                                              select new Dropdown.OptionData(resolution))
                                              .ToList();
        dropdown.ClearOptions();
        dropdown.AddOptions(dropList);
        dropdown.value = findCurrentSelection();
    }

    public void select(int item)
    {
        string[] dimensions = dropdown.options[item].text.Split(ResolutionDelimiter);
        Screen.SetResolution(int.Parse(dimensions[0]), int.Parse(dimensions[1]), Screen.fullScreen);
    }

    int findCurrentSelection()
    {
        string resolution = Screen.currentResolution.width.ToString() + ResolutionDelimiter + Screen.currentResolution.height.ToString();
        var options = dropdown.options;
        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].text == resolution)
                return i;
        }
        return 0;
    }
}
