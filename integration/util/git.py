import subprocess
import shutil


class GitClient:
    def __init__(self, working_dir):
        self.working_dir = working_dir

        # will prevent future command from throwing identity errors
        self.run('git config --global user.email "you@example.com"')
        self.run('git config --global user.name "Your Name"')

    def run(self, command):
        parts = command.split(" ")
        parts[0] = shutil.which("git")
        p = subprocess.Popen(parts, cwd=str(self.working_dir))
        p.wait()
        assert p.returncode == 0

    def create(self, filename):
        open(str(self.working_dir) + "/" + filename, "w").close()
