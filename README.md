# loop
Simulation of programming life

Add to .git/config on project file
[merge]
tool = unityyamlmerge

[mergetool "unityyamlmerge"]
trustExitCode = false
cmd = 'C:\Program Files\Unity\Hub\Editor\2022.1.23f1\Editor\Data\Tools\UnityYAMLMerge.exe' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
[branch "smart-merge"]
    remote = origin
    merge = refs/heads/smart-merge 
==========================================================================
Run git mergetool on git bash with directory of project file ex. D:\Unity\Repo\loop 
