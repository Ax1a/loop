# LOOP
Simulation of programming life

Add to .git/config on project file<br>
==========================================================================
<p>[merge]<br>
tool = unityyamlmerge<br>

[mergetool "unityyamlmerge"]<br>
trustExitCode = false<br>
cmd = 'C:\\Program Files\\Unity\Hub\\Editor\\2022.1.23f1\\Editor\\Data\\Tools\\UnityYAMLMerge.exe' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"<br>
[branch "smart-merge"]<br>
    remote = origin<br>
    merge = refs/heads/smart-merge </p>
    
Run git mergetool on git bash with directory of project file ex. <i>D:\Unity\Repo\loop</i>
==========================================================================

