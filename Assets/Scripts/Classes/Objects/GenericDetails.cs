using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GenericDetails
{
    [SerializeField] private int detailID;
    [SerializeField] private string detailCategory;
    [SerializeField] private string articleName;
    [SerializeField] private string articleDescription;
    [SerializeField] private string articleAuthors;
    [SerializeField] private string articleLink;

    //Ctrl Shift P then search generate set and get methods

    public GenericDetails(int detailID, string detailCategory, string articleName, string articleDescription, string articleAuthors, string articleLink) {
        this.detailID = detailID;
        this.articleName = articleName;
        this.articleDescription = articleDescription;
        this.articleAuthors = articleAuthors;
        this.articleLink = articleLink;
        this.detailCategory = detailCategory;
    }

    // ==============================================================================================================
    // |                                    Code to get the extra proposal info                                     |
    // ==============================================================================================================

    public int getDetailID()
    {
        return this.detailID;
    }

    public string getDetailCategory()
    {
        return this.detailCategory;
    }

    public string getArticleName()
    {
        return this.articleName;
    }

    public string getArticleDescription()
    {
        return this.articleDescription;
    }

    public string getArticleAuthors()
    {
        return this.articleAuthors;
    }

    public string getArticleLink()
    {
        return this.articleLink;
    }
}
