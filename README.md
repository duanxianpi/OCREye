<p align="center">
  <img src="Icon.ico" height="150" />
</p>
<p align="center">
  A OCR Software based on <a href="https://cloud.google.com/vision/docs/ocr">Google Cloud Platform</a>
</p>
<p align="center">
  <a href="https://github.com/duanxianpi/OCREye/blob/main/LICENSE.md">  
    <img alt="GitHub" src="https://img.shields.io/github/license/duanxianpi/OCREye?label=License">
  </a>
  <a href="https://github.com/duanxianpi/OCREye">
    <img alt="GitHub Repo stars" src="https://img.shields.io/github/stars/duanxianpi/OCREye"/>
  </a>
  <a href="https://github.com/duanxianpi/OCREye/releases">
    <img alt="GitHub release (latest by date including pre-releases)" src="https://img.shields.io/github/v/release/duanxianpi/OCREye?include_prereleases&sort=semver">
  </a>
  <a href="https://github.com/duanxianpi">
    <img alt="GitHub user" src="https://img.shields.io/badge/author-duanxianpi-brightgreen"/>
  </a>
</p>

## Introduction
OCREye is a Simple WPF-based OCR application. The OCR part is support by Google Cloud Platform.

## Feature
* Using GCP
* Physical effects (https://github.com/Blueve/Physics2D)
* Easy to use

## Usage
1. Right Click move the Eye

    ![HUXUIJ.gif](https://s4.ax1x.com/2022/02/11/HUXUIJ.gif)

2. Left Click start to take a screenshot 

    * **Please set config.json before using. If it isn't exist, creat one at the same level with applicantion)**
    ```Json
    {
        "snipPath":""
    }
    ```
    * **Please make sure you set up the service account of GCP before using)**


    ![HUXtZF.gif](https://s4.ax1x.com/2022/02/11/HUXtZF.gif)

3. Scroll the wheel will change the size of Eye

    ![HUXNa4.gif](https://s4.ax1x.com/2022/02/11/HUXNa4.gif)

