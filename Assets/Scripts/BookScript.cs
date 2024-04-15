using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class BookScript : MonoBehaviour
{
	[SerializeField, TabGroup("Scriptables")] List<OrderScriptable> scriptables;

	[SerializeField, TabGroup("TextMeshPro")] TextMeshPro page1, page2, page1Dupe, page2Dupe;

	[SerializeField] Animator anim;

	[ShowInInspector] int pageIndex = 0;
	[SerializeField] int pageFlipAmount = 2;

	bool nextLast = false, prevLast = false;

	void Start()
	{
		CalcPage(false);
		PreviousPages();
	}

	[Button(ButtonSizes.Large)]
	public void ToggleFwdPage()
	{
		CalcPage(true);
		NextPages();
	}

	[Button(ButtonSizes.Large)]
	public void ToggleBwdPage()
	{
		CalcPage(false);
		PreviousPages();
	}

	public void OpenBook() => anim.SetBool("OpenBook", true);
	public void CloseBook() => anim.SetBool("OpenBook", false);

	private void CalcPage(bool flipRight)
	{
		if (flipRight)
		{
			if (pageIndex + pageFlipAmount + 1 < scriptables.Count)
			{
				pageIndex += pageFlipAmount;
				page2.text = GetText(1);
			}
			else if (pageIndex + pageFlipAmount < scriptables.Count)
			{
				pageIndex += pageFlipAmount;
				page2.text = "";
			}
		}
		else
		{
			if (pageIndex - pageFlipAmount > -1 && pageIndex - 1 < scriptables.Count)
			{
				pageIndex -= pageFlipAmount;
			}
			
			page1.text = GetText(0);
		}
	}

	private void NextPages()
	{
		if (pageIndex + pageFlipAmount < scriptables.Count)
		{
			page2Dupe.text = GetText(-1);
			page1Dupe.text = GetText(0);
			anim.SetTrigger("NextPg");
			prevLast = false;
			nextLast = false;
		}
		else if (pageIndex < scriptables.Count && !nextLast)
		{
			page2Dupe.text = GetText(-1);
			page1Dupe.text = GetText(0);
			anim.SetTrigger("NextPg");
			prevLast = false;
			nextLast = true;
		}
		else
		{
			prevLast = false;
			nextLast = true;
		}
	}

	private void PreviousPages()
	{
		if (pageIndex - pageFlipAmount > -1)
		{
			page1Dupe.text = GetText(pageFlipAmount);
			page2Dupe.text = GetText(1);
			anim.SetTrigger("PreviousPg");
			prevLast = false;
			nextLast = false;
		}
		else if (!prevLast)
		{
			page1Dupe.text = GetText(pageFlipAmount);
			page2Dupe.text = GetText(1);
			anim.SetTrigger("PreviousPg");
			prevLast = true;
			nextLast = false;
		}
	}

	public void SetNextText()
	{
		page1.text = GetText(0);
	}
	public void SetPriorText()
	{
		page2.text = GetText(1);
	}
	
	private string GetText(int pageMod)
	{
		return $"{scriptables[pageIndex + pageMod].drinkName} \n{scriptables[pageIndex + pageMod].red * 100f}% Red \n{scriptables[pageIndex + pageMod].green * 100f}% Green \n{scriptables[pageIndex + pageMod].blue * 100f}% Blue ";
	}
}
